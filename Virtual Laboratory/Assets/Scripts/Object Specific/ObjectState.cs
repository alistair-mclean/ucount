///<summary>
/// ObjectState.cs - Container class for an interactable object's state. 
/// This activates and deactivates desired components of the gameobject. 
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] // hopefully this doesn't cause any errors when I disable the rigidbody component 
public class ObjectState : MonoBehaviour {
  //Public
  public enum State {Active, Deactive, Idle};

  //Private
  GameObject _object;
  private State _objectState = State.Idle; //Default state

  private void Start()
  {
    _object = GetComponent<GameObject>();
  }

  /// <summary>
  /// Active state of the object. Set once the user has clicked/touched on 
  /// an interactable object. Makes it much easier to move the object around. 
  /// </summary>
  public void SetActive()
  {
    // Re-enable the object if it has been disabled. And activate it
    if (_objectState == State.Deactive)
      _object.SetActive(true);
    _objectState = State.Active;

    // If the object is buoyant, disable that. 
    if (_object.GetComponent<Buoyancy>() != null)
    {
      _object.GetComponent<Buoyancy>().enabled = false;
    }
    // Cannot simply disable rigidbody component. So just disable gravity, and collisions. 
    _object.GetComponent<Rigidbody>().useGravity = false;
    _object.GetComponent<Rigidbody>().detectCollisions = false;
  }

/// <summary>
/// Default object state. 
/// </summary>
  public void SetIdle()
  {
    _objectState = State.Idle;
    // Re-enable the buoyancy (if applicable) 
    if (_object.GetComponent<Buoyancy>() != null)
    {
      _object.GetComponent<Buoyancy>().enabled = true;
    }
    // Re-enable gravity and collisions. 
    _object.GetComponent<Rigidbody>().useGravity = true;
    _object.GetComponent<Rigidbody>().detectCollisions = true;
  }


  /// <summary>
  /// Deactivate the object.
  /// </summary>
  public void SetDeactive()
  {
    _objectState = State.Deactive;
    _object.SetActive(true);
  }

  /// <summary>
  /// Accessor for object state
  /// </summary>
  /// <returns> The state of the object.</returns>
  public State GetObjectState()
  {
    return _objectState;
  }

}
