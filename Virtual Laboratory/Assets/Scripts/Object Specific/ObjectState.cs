///<summary>
/// ObjectState.cs - Container class for an interactable object's state. 
/// This activates and deactivates desired components of the gameobject. 
/// 
/// Copyright - VARIAL Studios LLC 
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] // hopefully this doesn't cause any errors when I disable the rigidbody component 
public class ObjectState : MonoBehaviour {
  //Public
  public enum State {Active, Deactive, Idle};

  //Private
  private State _objectState = State.Idle; //Default state
  

  /// <summary>
  /// Active state of the object. Set once the user has clicked/touched on 
  /// an interactable object. Makes it much easier to move the object around. 
  /// </summary>
  public void SetStateActive()
  {
    Debug.Log(name + " is in state: Active");
    // Re-enable the object if it has been disabled. And activate it
    if (_objectState == State.Deactive)
      gameObject.SetActive(true);
    _objectState = State.Active;

    // If the object is buoyant, disable that. 
    if (GetComponent<Buoyancy>() != null)
    {
      GetComponent<Buoyancy>().enabled = false;
    }
    // Cannot simply disable rigidbody component. So just disable gravity, and collisions. 
    GetComponent<Rigidbody>().useGravity = false;
    GetComponent<Rigidbody>().detectCollisions = false;
  }

/// <summary>
/// Default object state. 
/// </summary>
  public void SetStateIdle()
  {
    Debug.Log(name + " is in state: Idle");
    _objectState = State.Idle;
    
    // Re-enable gravity and collisions. 
    GetComponent<Rigidbody>().useGravity = true;
    GetComponent<Rigidbody>().detectCollisions = true;

    // Re-enable the buoyancy (if applicable) 
    if (GetComponent<Buoyancy>() != null || GetComponent<Buoyancy>().enabled == false)
    {
      GetComponent<Buoyancy>().enabled = true;
    }
  }


  /// <summary>
  /// Deactivate the object.
  /// </summary>
  public void SetStateDeactive()
  {
    Debug.Log(name + " is in state: Deactive");
    _objectState = State.Deactive;
    gameObject.SetActive(false);
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
