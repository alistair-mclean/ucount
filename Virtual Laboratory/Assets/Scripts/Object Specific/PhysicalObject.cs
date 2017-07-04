using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalObject : MonoBehaviour {
  // DESCRIPTION - Container class for commonly used physical descriptions 
  // of objects (i.e. - Volume, density, etc.)


  // Public
  public float AdjustmentConstant = 1.0f;
  public float ObjectMass;
  public float ObjectDensity;
  public float ObjectVolume;
  public Vector3 ObjectDimensions;
  

  // Private
  private Rigidbody _thisObject;
  private float _initialObjectMass;
  private float _initialObjectDensity;
  private Vector3 _initialObjectDimensions;
  private bool _objectUpdated = false; // has the user edited the object parameters? 

  private void Start()
  {
    // Save object's initial parameters
    _thisObject = GetComponent<Rigidbody>();
    _initialObjectMass = _thisObject.mass;
    _initialObjectDimensions = _thisObject.transform.localScale;

    // Initialize the public variables
    ObjectMass = _initialObjectMass;
    ObjectDimensions = _initialObjectDimensions;
    ObjectVolume = ObjectDimensions.x * ObjectDimensions.y * ObjectDimensions.z;
    ObjectDensity = ObjectMass / ObjectVolume;

    // Debugging
    Debug.Log("Object Mass" + ObjectMass);
    Debug.Log("Object dimensions = ( " + ObjectDimensions.x + ", " + ObjectDimensions.y + ", " + ObjectDimensions.z + " ) ");
    Debug.Log("Object Volume = " + ObjectVolume);
    Debug.Log("Object Density = " + ObjectDensity);
  }


  void LateUpdate () {

  }

}
