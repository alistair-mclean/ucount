// Buoyancy.cs
// by Alex Zhdankin
// edited by Alistair McLean
// Version 2.1
//
// http://forum.unity3d.com/threads/72974-Buoyancy-script
//
// Terms of use: do whatever you like. <- Thanks Alex

using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{
  // DESCRIPTION - Adds buoyancy to the assigned object. Calculates an Archimedes force on a list of voxels that are 
  // created. These voxels are a good way to create a robust buoyancy for a complicated mesh

  // Public
  public GameObject Liquid; // This must be assigned by the developer 
	public float ObjectDensity = 500;
  public float LiquidDensity = 1000; //Default density of water 
  public float SubmergedVolume = 0;
	public int SlicesPerAxis = 2;
	public bool IsConcave = false;
  public bool BuoyancyIsActive = true;
	public int VoxelsLimit = 16;

  // Private 
	private const float _DAMPFER = 0.2f;
  private float _maximumSubmergedVolume = 0;
  private float _voxelHalfHeight;
	private Vector3 _localArchimedesForce;
	private List<Vector3> _voxels;
	private bool _isMeshCollider;
	private List<Vector3[]> _forces; // For drawing force gizmos

	/// <summary>
	/// Provides initialization.
	/// </summary>
	private void Start()
	{
    if (Liquid.GetComponent<Liquid>() == null)
    {
      Debug.LogError("Error in Buoyancy Component for " + gameObject.name + " NO LIQUID COMPONENT IN LIQUID.");
    }
    else
    { 
      LiquidDensity = Liquid.GetComponent<Liquid>().Density;
      Debug.Log("Detected liquid Density = " + LiquidDensity);
    }
    _forces = new List<Vector3[]>(); // For drawing force gizmos

		// Store original rotation and position
		var originalRotation = transform.rotation;
		var originalPosition = transform.position;
		transform.rotation = Quaternion.identity;
		transform.position = Vector3.zero;

		// The object must have a collider
		if (GetComponent<Collider>() == null)
		{
			gameObject.AddComponent<MeshCollider>();
			Debug.LogWarning(string.Format("[Buoyancy.cs] Object \"{0}\" had no collider. MeshCollider has been added.", name));
		}
		_isMeshCollider = GetComponent<MeshCollider>() != null;

    var bounds = GetComponent<Collider>().bounds;
    _maximumSubmergedVolume = bounds.size.magnitude;
    if (bounds.size.x < bounds.size.y)
		{
			_voxelHalfHeight = bounds.size.x;
		}
		else
		{
			_voxelHalfHeight = bounds.size.y;
		}
		if (bounds.size.z < _voxelHalfHeight)
		{
			_voxelHalfHeight = bounds.size.z;
		}
		_voxelHalfHeight /= 2 * SlicesPerAxis;

		// The object must have a RidigBody
		if (GetComponent<Rigidbody>() == null)
		{
			gameObject.AddComponent<Rigidbody>();
			Debug.LogWarning(string.Format("[Buoyancy.cs] Object \"{0}\" had no Rigidbody. Rigidbody has been added.", name));
		}
		GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -bounds.extents.y * 0f, 0) + transform.InverseTransformPoint(bounds.center);

		_voxels = SliceIntoVoxels(_isMeshCollider && IsConcave);

		// Restore original rotation and position
		transform.rotation = originalRotation;
		transform.position = originalPosition;

		float volume = GetComponent<Rigidbody>().mass / ObjectDensity;

		WeldPoints(_voxels, VoxelsLimit);

		float archimedesForceMagnitude = LiquidDensity * Mathf.Abs(Physics.gravity.y) * volume;
		_localArchimedesForce = new Vector3(0, archimedesForceMagnitude, 0) / _voxels.Count;

		Debug.Log(string.Format("[Buoyancy.cs] Name=\"{0}\" volume={1:0.0}, mass={2:0.0}, density={3:0.0}", name, volume, GetComponent<Rigidbody>().mass, ObjectDensity));
	}

	/// <summary>
	/// Slices the object into number of voxels represented by their center points.
	/// <param name="concave">Whether the object have a concave shape.</param>
	/// <returns>List of voxels represented by their center points.</returns>
	/// </summary>
	private List<Vector3> SliceIntoVoxels(bool concave)
	{
		var points = new List<Vector3>(SlicesPerAxis * SlicesPerAxis * SlicesPerAxis);

		if (concave)
		{
			var meshCol = GetComponent<MeshCollider>();

			var convexValue = meshCol.convex;
			meshCol.convex = false;

			// Concave slicing
			var bounds = GetComponent<Collider>().bounds;
			for (int ix = 0; ix < SlicesPerAxis; ix++)
			{
				for (int iy = 0; iy < SlicesPerAxis; iy++)
				{
					for (int iz = 0; iz < SlicesPerAxis; iz++)
					{
						float x = bounds.min.x + bounds.size.x / SlicesPerAxis * (0.5f + ix);
						float y = bounds.min.y + bounds.size.y / SlicesPerAxis * (0.5f + iy);
						float z = bounds.min.z + bounds.size.z / SlicesPerAxis * (0.5f + iz);

						var p = transform.InverseTransformPoint(new Vector3(x, y, z));

						if (PointIsInsideMeshCollider(meshCol, p))
						{
							points.Add(p);
						}
					}
				}
			}
			if (points.Count == 0)
			{
				points.Add(bounds.center);
			}

			meshCol.convex = convexValue;
		}
		else
		{
			// Convex slicing
			var bounds = GetComponent<Collider>().bounds;
			for (int ix = 0; ix < SlicesPerAxis; ix++)
			{
				for (int iy = 0; iy < SlicesPerAxis; iy++)
				{
					for (int iz = 0; iz < SlicesPerAxis; iz++)
					{
						float x = bounds.min.x + bounds.size.x / SlicesPerAxis * (0.5f + ix);
						float y = bounds.min.y + bounds.size.y / SlicesPerAxis * (0.5f + iy);
						float z = bounds.min.z + bounds.size.z / SlicesPerAxis * (0.5f + iz);

						var p = transform.InverseTransformPoint(new Vector3(x, y, z));

						points.Add(p);
					}
				}
			}
		}

		return points;
	}

	/// <summary>
	/// Returns whether the point is inside the mesh collider.
	/// </summary>
	/// <param name="c">Mesh collider.</param>
	/// <param name="p">Point.</param>
	/// <returns>True - the point is inside the mesh collider. False - the point is outside of the mesh collider. </returns>
	private static bool PointIsInsideMeshCollider(Collider c, Vector3 p)
	{
		Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

		foreach (var ray in directions)
		{
			RaycastHit hit;
			if (c.Raycast(new Ray(p - ray * 1000, ray), out hit, 1000f) == false)
			{
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Returns two closest points in the list.
	/// </summary>
	/// <param name="list">List of points.</param>
	/// <param name="firstIndex">Index of the first point in the list. It's always less than the second index.</param>
	/// <param name="secondIndex">Index of the second point in the list. It's always greater than the first index.</param>
	private static void FindClosestPoints(IList<Vector3> list, out int firstIndex, out int secondIndex)
	{
		float minDistance = float.MaxValue, maxDistance = float.MinValue;
		firstIndex = 0;
		secondIndex = 1;

		for (int i = 0; i < list.Count - 1; i++)
		{
			for (int j = i + 1; j < list.Count; j++)
			{
				float distance = Vector3.Distance(list[i], list[j]);
				if (distance < minDistance)
				{
					minDistance = distance;
					firstIndex = i;
					secondIndex = j;
				}
				if (distance > maxDistance)
				{
					maxDistance = distance;
				}
			}
		}
	}

	/// <summary>
	/// Welds closest points.
	/// </summary>
	/// <param name="list">List of points.</param>
	/// <param name="targetCount">Target number of points in the list.</param>
	private static void WeldPoints(IList<Vector3> list, int targetCount)
	{
		if (list.Count <= 2 || targetCount < 2)
		{
			return;
		}

		while (list.Count > targetCount)
		{
			int first, second;
			FindClosestPoints(list, out first, out second);

			var mixed = (list[first] + list[second]) * 0.5f;
			list.RemoveAt(second); // the second index is always greater that the first => removing the second item first
			list.RemoveAt(first);
			list.Add(mixed);
		}
	}

	/// <summary>
	/// Returns the water level at given location.
	/// </summary>
	/// <param name="x">x-coordinate</param>
	/// <param name="z">z-coordinate</param>
	/// <returns>Water level</returns>
	private float GetWaterLevel(float x, float z)
	{
    //		return ocean == null ? 0.0f : ocean.GetWaterHeightAtLocation(x, z);
    if (Liquid != null ) {
      Bounds liquidBounds = Liquid.GetComponent<Collider>().bounds;
      // If the object is not inside of the bounds of the liquid, the height is 0. 
      if (x > liquidBounds.max.x || z > liquidBounds.max.z)
        return 0;
      if (x < liquidBounds.min.x || z < liquidBounds.min.z)
        return 0;
      return liquidBounds.size.y; 
    }
    else 
      return 0.0f;
	}

	/// <summary>
	/// Calculates physics.
	/// </summary>
	private void FixedUpdate()
	{
		_forces.Clear(); // For drawing force gizmos
    if (BuoyancyIsActive)
    {
      foreach (var point in _voxels)
      {
        var wp = transform.TransformPoint(point);
        float waterLevel = GetWaterLevel(wp.x, wp.z);

        //HACK TO FIX THE OBJECT FROM FLOATING OUTSIDE OF THE BOUNDS - Alistair
        if (waterLevel <= 0)
          return;
        // hack end
        if (wp.y - _voxelHalfHeight < waterLevel)
        {
          float k = (waterLevel - wp.y) / (2 * _voxelHalfHeight) + 0.5f;
          if (k > 1)
          {
            k = 1f;
          }
          else if (k < 0)
          {
            k = 0f;
          }
          if (SubmergedVolume < _maximumSubmergedVolume)
            SubmergedVolume += Mathf.Pow(_voxelHalfHeight, 3);
          var velocity = GetComponent<Rigidbody>().GetPointVelocity(wp);
          var localDampingForce = -velocity * _DAMPFER * GetComponent<Rigidbody>().mass;
          var force = localDampingForce + Mathf.Sqrt(k) * _localArchimedesForce;
          GetComponent<Rigidbody>().AddForceAtPosition(force, wp);

          _forces.Add(new[] { wp, force }); // For drawing force gizmos
        }
      }
    }
  }

  // TEMPORARY
  public float GetSubmergedVolume()
  {
    return SubmergedVolume;
  }


	/// <summary>
	/// Draws gizmos.
	/// </summary>
	private void OnDrawGizmos()
	{
		if (_voxels == null || _forces == null)
		{
			return;
		}

		const float gizmoSize = 0.05f;
		Gizmos.color = Color.yellow;


		foreach (var p in _voxels)
		{
			Gizmos.DrawCube(transform.TransformPoint(p), new Vector3(gizmoSize, gizmoSize, gizmoSize));
		}

		Gizmos.color = Color.cyan;

		foreach (var force in _forces)
		{
			Gizmos.DrawCube(force[0], new Vector3(gizmoSize, gizmoSize, gizmoSize));
			Gizmos.DrawLine(force[0], force[0] + force[1] / GetComponent<Rigidbody>().mass);
		}
	}
}