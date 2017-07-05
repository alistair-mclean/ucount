using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// DESCRIPTION - This class controls the behavior of a liquid. 
// Height control, and fluid density are controlled through this script. 

public class Liquid : MonoBehaviour {
  // Public
  public float Density = 1000f; // in kg/m^3
  public float DragCoefficient = 0.3f; // Arbitrary value (for now)

  // Private
  private Collider _liquidCollider;
  private Vector3 _initialDimensions;
  private float _initialVolume;
  private float _liquidVolume;
  private List<GameObject> _collidingObjects;
  


  private void Start()
  {
    _collidingObjects = new List<GameObject>();
    if (gameObject.GetComponent<Collider>() == null)
    {
      Debug.LogError("Error in Liquid class for" + gameObject.name + ". NO COLLIDER COMPONENT");
    }
    else
      _liquidCollider = gameObject.GetComponent<Collider>();
    _initialDimensions = gameObject.transform.localScale;
    _initialVolume = _initialDimensions.x * _initialDimensions.y * _initialDimensions.z;
    _liquidVolume = _initialVolume;
  }

  private void OnCollisionEnter(Collision collision)
  {
    GameObject collidingObject = collision.gameObject;
  }

  private void OnTriggerEnter(Collider other)
  {
    GameObject collidingObject = other.gameObject;
    if (collidingObject.GetComponent<Buoyancy>() == null)
    {
      Debug.Log("Non-Buoyant object triggering liquid");
    }
    else
    {
      if (!_collidingObjects.Contains(collidingObject))
      {
        _collidingObjects.Add(collidingObject);
      }
      Buoyancy buoyantObject = collidingObject.GetComponent<Buoyancy>();
    }
  }

  private void OnTriggerExit(Collider other)
  {
    // Remove the gameobject from the collidedObjects list 
    GameObject collidedObject = other.gameObject;
    if (_collidingObjects.Contains(collidedObject))
    {
      _collidingObjects.Remove(collidedObject);
    }
  }

  private void Update()
  {
    CalculateLiquidDimensions();
  }

  // Calculate the new liquid level each frame. 
  private void CalculateLiquidDimensions()
  {
    float totalSubmergedVolume = 0.0f;
    float totalVolume = _initialVolume;
    Vector3 newDimensions = new Vector3();
    int objectCounter = 0; // DEBUG
    foreach (GameObject submergedObject in _collidingObjects)
    {
      totalSubmergedVolume += submergedObject.GetComponent<Buoyancy>().GetSubmergedVolume();
      objectCounter++;
    }
    Debug.Log("Liquid.cs - Number of objects in liquid: " + objectCounter);
    Debug.Log("Liquid.cs - Total Submerged Volume: " + totalSubmergedVolume);
    totalVolume += totalSubmergedVolume;
    _liquidVolume = totalVolume;
    float newHeight = totalVolume / (_initialDimensions.x * _initialDimensions.y);
//    Debug.Log("New hegiht = " + newHeight + ", totalVolume = " + totalVolume); //DEBUG
    newDimensions = _initialDimensions;
    newDimensions.z += newHeight;
    transform.localScale = Vector3.Lerp(_initialDimensions, newDimensions, Time.deltaTime) ;
  }

  private void AddDragToObjectsInList()
  {
    foreach (GameObject submergedObject in _collidingObjects)
    {
      if(submergedObject.GetComponent<Rigidbody>() == null)
      {
        Debug.Log("Submerged object: " + submergedObject.name + " is not a rigid body. Will not apply drag.");
        return;
      }
      Rigidbody submergedRigidBody = submergedObject.GetComponent<Rigidbody>();
      if(submergedObject.GetComponent<Buoyancy>() == null)
      {
        Debug.Log("Submerged object: " + submergedObject.name + " is not buoyant. Will not apply drag.");
        return;
      }
      Buoyancy buoyantObject = submergedObject.GetComponent<Buoyancy>();
      float objectDensity = buoyantObject.ObjectDensity;
      submergedRigidBody.drag = (Density / objectDensity) * buoyantObject.GetSubmergedVolume();
      Debug.Log("Liquid.cs - Submerged object: " + submergedObject.name + " has drag: " + submergedRigidBody.drag); // DEBUG
    }
  }
  
  public float GetLiquidVolume()
  {
    return _liquidVolume;
  }

}
