///<summary>
/// Liquid.cs - Controls liquid parameters, and interactions with buoyant objects.
/// 
/// Copyright - VARIAL Studios LLC 
///</summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class Liquid : MonoBehaviour {

  // Public
  public float Density = 1000f; // in kg/m^3
  public float RiseTimeConstant = 0.3f; //Arbitrary constant to fine tune how quickly the liquid rises 
  public float DragCoefficient = 1.3f;
  public float AngularDragCoefficient = 1.2f;

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

  // When an object enters the liquid, add it to the list of collidingobjects
  private void OnTriggerEnter(Collider other)
  {
    GameObject collidingObject = other.gameObject;
    // If the colliding object is buoyant, add it to the list to later calculate how much to raise the water level
    if (collidingObject.GetComponent<Buoyancy>() != null)
    {
      if (!_collidingObjects.Contains(collidingObject))
      {
        _collidingObjects.Add(collidingObject);
      }
      Buoyancy buoyantObject = collidingObject.GetComponent<Buoyancy>();
    }
    if (collidingObject.GetComponent<Rigidbody>() != null) {
      collidingObject.GetComponent<Rigidbody>().drag = DragCoefficient;
      collidingObject.GetComponent<Rigidbody>().angularDrag = AngularDragCoefficient;
    }

  }

  private void OnTriggerExit(Collider other)
  {
    // Remove the gameobject from the collidedObjects list 
    GameObject collidedObject = other.gameObject;
    if (_collidingObjects.Contains(collidedObject))
    {
      collidedObject.GetComponent<Rigidbody>().drag = 0;
      collidedObject.GetComponent<Rigidbody>().angularDrag = 0;
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
    newDimensions = _initialDimensions;
    newDimensions.z += newHeight;
    transform.localScale = Vector3.Lerp(_initialDimensions, newDimensions, Time.deltaTime * RiseTimeConstant) ;
  }
  
  public float GetLiquidVolume()
  {
    return _liquidVolume;
  }
  
  public void SetLiquidDensity(float newDensity)
  {
    Density = newDensity;
  }
}
