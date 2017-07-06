using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Liquid : MonoBehaviour {
  // DESCRIPTION - This class controls the behavior of a liquid. 
  // Height control, and fluid density are controlled through this script. 

  // Public
  public float Density = 1000f; // in kg/m^3
  public float RiseTimeConstant = 0.3f; //Arbitrary constant to fine tune how quickly the liquid rises 

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
    Debug.Log(collidingObject.name + " Colliding with Liquid");
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

  private void FixedUpdate()
  {
    CalculateLiquidDimensions();
  }

  // Calculate the new liquid level each frame. 
  private void CalculateLiquidDimensions()
  {
    float totalSubmergedVolume = 0.0f;
    float totalVolume = _initialVolume;
    Vector3 newDimensions = new Vector3();
    foreach (GameObject submergedObject in _collidingObjects)
    {
      totalSubmergedVolume += submergedObject.GetComponent<Buoyancy>().GetSubmergedVolume();
    }
    totalVolume += totalSubmergedVolume;
    _liquidVolume = totalVolume;
    float newHeight = totalVolume / (_initialDimensions.x * _initialDimensions.y);
    Debug.Log("New hegiht = " + newHeight + ", totalVolume = " + totalVolume);
    newDimensions = _initialDimensions;
    newDimensions.z += newHeight;
    transform.localScale = Vector3.Lerp(_initialDimensions, newDimensions, Time.deltaTime * RiseTimeConstant) ;
  }
  
  public float GetLiquidVolume()
  {
    return _liquidVolume;
  }

}
