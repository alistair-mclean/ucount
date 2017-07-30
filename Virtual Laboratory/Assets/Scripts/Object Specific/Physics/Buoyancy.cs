///<summary>
/// Buoyancy.cs - Adds buoyancy to the assigned object. Calculates an Archimedes force on a list of voxels that are 
/// created based on the assigned coordinate system. These voxels are a good way to create a robust buoyancy for a complicated mesh.
/// </summary>
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshCollider))]
public class Buoyancy : MonoBehaviour
{
  // Public
  public enum CoordinateType {Cartesian, Cylindrical, Spherical}; //Enumerator for the type of coordinate system to apply to an object
  public CoordinateType CoordinateSystem; // The assigned coordinate system 
  public GameObject Liquid; // This must be assigned by the developer 
	public float ObjectDensity = 500;
  public float LiquidDensity = 1000; //Default density of water 
	public int SlicesPerDimension = 2;
	public bool IsConcave = false;
  public bool BuoyancyIsActive = true;
	public int VoxelsLimit = 16;

  // Private 
  private const float _DAMPFER = 0.2f;
  private float _maximumSubmergedVolume = 0.0f;
  private float _submergedVolume = 0;
  private float _netBuoyantForce = 0.0f;
  private float _volume;
  private float _voxelHalfHeight;
	private Vector3 _localArchimedesForce;
  private Vector3 _acceleration;
  private Vector3 _lastPosition;
	private List<Vector3> _voxels;
	private bool _isMeshCollider;
	private List<Vector3[]> _forces; // For drawing force gizmos

  /// <summary>
  /// Provides initialization.
  /// </summary>
  private void Start()
  {
    _lastPosition = transform.position;
    _acceleration = Vector3.zero;
    CalculateInitialObjectVolume();
    if (Liquid.GetComponent<Liquid>() == null)
    {
      Debug.LogError("Error in Buoyancy Component for " + gameObject.name + " NO LIQUID COMPONENT IN LIQUID.");
    }
    else
    {
      LiquidDensity = Liquid.GetComponent<Liquid>().Density;
    }
    _forces = new List<Vector3[]>(); // For drawing force gizmos

    // Store original rotation and position
    var originalRotation = transform.rotation;
    var originalPosition = transform.position;
    transform.rotation = Quaternion.identity;
    transform.position = Vector3.zero;


    var bounds = GetComponent<Collider>().bounds;
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
    _voxelHalfHeight /= 2 * SlicesPerDimension;

    gameObject.GetComponent<Rigidbody>().mass = _volume * ObjectDensity;
    GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -bounds.extents.y * 0f, 0) + transform.InverseTransformPoint(bounds.center);

    switch (CoordinateSystem) {
      case (CoordinateType.Cartesian):
        _voxels = CartesianSliceIntoVoxels(IsConcave);
        break;
      case (CoordinateType.Spherical):
        _voxels = SphericalSliceIntoVoxels(IsConcave);
        break;
      case (CoordinateType.Cylindrical):
        break;
    };
		// Restore original rotation and position
		transform.rotation = originalRotation;
		transform.position = originalPosition;


		WeldPoints(_voxels, VoxelsLimit);

		float archimedesForceMagnitude = LiquidDensity * Mathf.Abs(Physics.gravity.y) * _volume;
		_localArchimedesForce = new Vector3(0, archimedesForceMagnitude, 0) / _voxels.Count;

		//Debug.Log(string.Format("[Buoyancy.cs] Name=\"{0}\" volume={1:0.0}, mass={2:0.0}, density={3:0.0}", name, _volume, GetComponent<Rigidbody>().mass, ObjectDensity));
	}

	/// <summary>
	/// Slices the object into number of voxels based on a cartesian 
  /// system coordinate represented by their center points.
	/// <param name="concave">Whether the object have a concave shape.</param>
	/// <returns>List of voxels represented by their center points.</returns>
	/// </summary>
	private List<Vector3> CartesianSliceIntoVoxels(bool concave)
	{
		var points = new List<Vector3>(SlicesPerDimension * SlicesPerDimension * SlicesPerDimension);

		if (concave)
		{
			var meshCol = GetComponent<MeshCollider>();

			var convexValue = meshCol.convex;
			meshCol.convex = false;

			// Concave slicing
			var bounds = GetComponent<Collider>().bounds;
			for (int ix = 0; ix < SlicesPerDimension; ix++)
			{
				for (int iy = 0; iy < SlicesPerDimension; iy++)
				{
					for (int iz = 0; iz < SlicesPerDimension; iz++)
					{
						float x = bounds.min.x + bounds.size.x / SlicesPerDimension * (0.5f + ix);
						float y = bounds.min.y + bounds.size.y / SlicesPerDimension * (0.5f + iy);
						float z = bounds.min.z + bounds.size.z / SlicesPerDimension * (0.5f + iz);

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
			for (int ix = 0; ix < SlicesPerDimension; ix++)
			{
				for (int iy = 0; iy < SlicesPerDimension; iy++)
				{
					for (int iz = 0; iz < SlicesPerDimension; iz++)
					{
						float x = bounds.min.x + bounds.size.x / SlicesPerDimension * (0.5f + ix);
						float y = bounds.min.y + bounds.size.y / SlicesPerDimension * (0.5f + iy);
						float z = bounds.min.z + bounds.size.z / SlicesPerDimension * (0.5f + iz);

						var p = transform.InverseTransformPoint(new Vector3(x, y, z));

						points.Add(p);
					}
				}
			}
		}

		return points;
	}

  /// <summary>
  /// Slice the mesh into a series of voxels based on a spherical coordinate 
  /// system. 
  /// </summary>
  /// <param name="concave"></param>
  /// <returns></returns>
  private List<Vector3> SphericalSliceIntoVoxels(bool concave)
  { 
    // IN CONSTRUCTION - BEGIN 
    // This needs some thinking
    List<Vector3> points = new List<Vector3>(SlicesPerDimension * SlicesPerDimension * SlicesPerDimension);
    if (concave)
    {
      MeshCollider meshCol = GetComponent<MeshCollider>();

      bool convexValue = meshCol.convex;
      convexValue = false;

      Bounds bounds = GetComponent<Collider>().bounds;
      
      // Phi
      //float maxRadius = bounds.max.magnitude;
      float maxRadius = transform.localScale.y / 2;
      float numberOfSlices = (float)SlicesPerDimension; //float conversion
      float phi = -(3.0f / 2.0f) * Mathf.PI;
      for (; phi <= Mathf.PI / 2.0f; phi += Mathf.PI / numberOfSlices)
      {
        // This loop needs to do the radial 
        for (float theta = 0.0f; theta <= 360.0f; theta += 360.0f / numberOfSlices)
        {
          // This loop needs to the radius coming inwards
          for (float radius = 0; radius <= maxRadius/2.0f; radius += maxRadius / numberOfSlices)
          {
            Vector3 centerOfObject = transform.position;
            float x = (bounds.center.x + radius) * Mathf.Cos(phi) * Mathf.Sin(theta);
            float y = (bounds.center.y + radius) * Mathf.Sin(phi) * Mathf.Sin(theta);
            float z = (bounds.center.z + radius) * Mathf.Cos(phi);

            //Vector3 newPoint = transform.InverseTransformPoint(new Vector3(x, y, z));
            Vector3 newPoint = new Vector3(x, y, z);
            if (PointIsInsideMeshCollider(meshCol, newPoint))
            {
              points.Add(newPoint);
            }
          }
        }
      }
    }
    else
    {
      Bounds bounds = GetComponent<Collider>().bounds;
      Vector3 maxRadiusVector = bounds.max - bounds.min;
      float maxRadius = maxRadiusVector.magnitude;
      //float maxRadius = Mathf.Pow(_volume * (3.0f / (4.0f * Mathf.PI)), (1.0f / 3.0f));
      float numberOfSlices = (float)SlicesPerDimension; //float conversion
      float phi = -(3.0f / 2.0f) * Mathf.PI;
      // Phi loop
      for (; phi <= Mathf.PI / 2.0f; phi += Mathf.PI / numberOfSlices)
      {
        // Theta loop 
        for (float theta = 0.0f; theta <= 360.0f; theta += 360.0f / numberOfSlices)
        {
          // Radius loop
          //          for (float radius = 0; radius <= bounds.min.magnitude; radius += bounds.min.magnitude / numberOfSlices)
          for (int radiusCounter = 0; radiusCounter < SlicesPerDimension; ++radiusCounter)
          {
            float radius = bounds.min.magnitude;
            Vector3 centerOfObject = transform.position;
            float x = bounds.center.x + radius * Mathf.Cos(phi) * Mathf.Sin(theta);
            float y = bounds.center.y + radius * Mathf.Sin(phi) * Mathf.Sin(theta);
            float z = bounds.center.z + radius * Mathf.Cos(phi);

            Vector3 newPoint = transform.InverseTransformPoint(new Vector3(x, y, z));
            if (PointIsInsideMeshCollider(GetComponent<Collider>(), newPoint)) 
              points.Add(newPoint);
            
          }
        }
      }
      if (points.Count == 0)
      {
        points.Add(bounds.center);
      }



    }
    return points;
  }
  
  // IN CONSTRUCTION - END

	/// <summary>
	/// Returns whether the point is inside the mesh collider.
	/// </summary>
	/// <param name="c">Mesh collider.</param>
	/// <param name="p">Point.</param>
	/// <returns>True - the point is inside the mesh collider. False - the point is outside of the mesh collider. </returns>
	private static bool PointIsInsideMeshCollider(Collider other, Vector3 point)
	{
		Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

		foreach (var ray in directions)
		{
			RaycastHit hit;
			if (other.Raycast(new Ray(point - ray * 1000, ray), out hit, 1000f) == false)
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
    LiquidDensity = Liquid.GetComponent<Liquid>().Density;
    _acceleration = transform.position - _lastPosition / Time.deltaTime; // Calculate instantaneous acceleration of object
    _netBuoyantForce = 0.0f;
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
          CalculateApparentSubmergedVolume();
          float archimedesForceMagnitude = LiquidDensity * Mathf.Abs(Physics.gravity.y) * _volume;
          _localArchimedesForce = new Vector3(0, archimedesForceMagnitude, 0) / _voxels.Count;
          var velocity = GetComponent<Rigidbody>().GetPointVelocity(wp);
          var localDampingForce = -velocity * _DAMPFER * GetComponent<Rigidbody>().mass;
          var force = localDampingForce + Mathf.Sqrt(k) * _localArchimedesForce;
          GetComponent<Rigidbody>().AddForceAtPosition(force, wp);
          _netBuoyantForce += force.y; // Sum up the overall vertical buoyant force for each object on each frame
          _forces.Add(new[] { wp, force }); // For drawing force gizmos
        }
      }
    }
    _lastPosition = transform.position;
  }

  // TEMPORARY
  public float GetSubmergedVolume()
  {
    return _submergedVolume;
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
  /// <summary>
  /// Calculate the initial volume based on the object type. 
  /// </summary>
  private void CalculateInitialObjectVolume()
  {
    switch (CoordinateSystem)
    {
      // Approximate max volume as a rectangle
      case (CoordinateType.Cartesian):
        _volume = transform.localScale.x * transform.localScale.y * transform.localScale.z;
        break;

      // Approximate max volume as a cylinder
      case (CoordinateType.Cylindrical):
        // Check to see which dimension is the largest, then approximate a radius 
        // based on the square magintude of the other two dimensions
        float length = 0.0f;
        float radius = 0.0f;
        if (transform.localScale.x > transform.localScale.y && transform.localScale.x > transform.localScale.z)
        {
          length = transform.localScale.x;
          float f = Mathf.Pow(transform.localScale.y, 2.0f) + Mathf.Pow(transform.localScale.z, 2.0f);
          radius = Mathf.Pow(f, 0.5f);
        }
        else if (transform.localScale.y > transform.localScale.x && transform.localScale.y > transform.localScale.z)
        {
          length = transform.localScale.y;
          float f = Mathf.Pow(transform.localScale.x, 2.0f) + Mathf.Pow(transform.localScale.z, 2.0f);
          radius = Mathf.Pow(f, 0.5f);
        }
        else
        {
          length = transform.localScale.z;
          float f = Mathf.Pow(transform.localScale.y, 2.0f) + Mathf.Pow(transform.localScale.z, 2.0f);
          radius = Mathf.Pow(f, 0.5f);
        }
        _volume = 2.0f * Mathf.PI * radius * length;
        break;

      // Approximate max volume as a sphere 
      case (CoordinateType.Spherical):
        // Approximate a radius from the root mean square of the initial dimensions
        float x = Mathf.Pow(transform.localScale.x, 2.0f);
        float y = Mathf.Pow(transform.localScale.y, 2.0f);
        float z = Mathf.Pow(transform.localScale.z, 2.0f);
        float temp = (1.0f / 3.0f) * (x + y + z);
        radius = Mathf.Pow(temp, 0.5f);
        _volume = (4.0f / 3.0f) * Mathf.PI * Mathf.Pow(radius, 3);
        break;
    }
  }

 
  /// <summary>
  /// Calculates the apparent submerged volume of the object. This is not by any means accurate. 
  /// This can very much be improved! 
  /// </summary>
  private void CalculateApparentSubmergedVolume()
  {
    float waterLevel = GetWaterLevel(transform.position.x, transform.position.y);
    float submergedLevel = 0.0f;
    float bottomOfObject = transform.position.y - transform.localScale.y / 2;
    submergedLevel = waterLevel - bottomOfObject;
    if (submergedLevel >= transform.localScale.y + transform.position.y)
      submergedLevel = transform.localScale.y;
    switch (CoordinateSystem)
    {
      case (CoordinateType.Cartesian):
        float crossSection = transform.localScale.x * transform.localScale.z;
        _submergedVolume = crossSection * submergedLevel;
        break;

      case (CoordinateType.Spherical):
        // Approximate a radius from the root mean square of the submerged height
        float tempRadius = Mathf.Pow(submergedLevel, 2.0f);
        _submergedVolume = (4.0f / 3.0f) * Mathf.PI * Mathf.Pow(tempRadius/2, 3.0f);
        break;

      case (CoordinateType.Cylindrical):
        // Soon to be aded ;)
        break;
    }
  }

  public void SetObjectDensity(float newDensity)
  {
    ObjectDensity = newDensity;
  }

  public float GetObjectDensity()
  {
    return ObjectDensity;
  }
}