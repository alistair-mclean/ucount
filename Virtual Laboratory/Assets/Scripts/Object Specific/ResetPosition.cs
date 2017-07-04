using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour {
  // DESCRIPTION - Resets the gameobect's position if its transform goes out of bounds.
  // With the ability to toggle the resetting of forces or not. Defaulted to yes. 
  // As a reminder, the y-direction is considered to be the vertical direction in all scenes!

  //Public
  public Bounds ResetBounds; // SUGGESTION - Change the way this works. Make a global gameobject called SceneBounds and on start, set that to whwat this is.
  public bool ResetVelocity = true;

  //Private
  private Rigidbody _object;
  private Vector3 _initialPosition = new Vector3(0.0f, 0.0f, 0.0f);
  private Vector3 _initialVelocity = new Vector3(0.0f, 0.0f, 0.0f);

  private void Start()
  { 
    _object = GetComponent<Rigidbody>();
    _initialPosition = transform.position;
    if(GetComponent<InitialForce>())
    {
      _object.AddForce(GetComponent<InitialForce>().ForceOnObject);
    }
  }

  void LateUpdate ()
  {
    //Reset if x is outside of bounds
    if (transform.position.x <= ResetBounds.min.x)
    {
      transform.position = _initialPosition;
      if (ResetVelocity)
      {
        _object.velocity = Vector3.zero;
        _object.angularVelocity = Vector3.zero;
      }
    }
    if (transform.position.x >= ResetBounds.max.x)
    {
      transform.position = _initialPosition;
      if (ResetVelocity)
      {
        _object.velocity = Vector3.zero;
        _object.angularVelocity = Vector3.zero;
      }
    }

    //Reset if y is outside of bounds
    if (transform.position.y <= ResetBounds.min.y)
    {
      transform.position = _initialPosition;
      if (ResetVelocity)
      {
        _object.velocity = Vector3.zero;
        _object.angularVelocity = Vector3.zero;
      }
    }
    if (transform.position.y >= ResetBounds.max.y)
    {
      transform.position = _initialPosition;
      if (ResetVelocity)
      {
        _object.velocity = Vector3.zero;
        _object.angularVelocity = Vector3.zero;
      }
    }

    //Reset if z is outside of bounds
    if (transform.position.z <= ResetBounds.min.z)
    {
      transform.position = _initialPosition;
      if (ResetVelocity)
      {
        _object.velocity = Vector3.zero;
        _object.angularVelocity = Vector3.zero;
      }
    }
    if (transform.position.z >= ResetBounds.max.z)
    {
      transform.position = _initialPosition;
      if (ResetVelocity)
      {
        _object.velocity = Vector3.zero;
        _object.angularVelocity = Vector3.zero;
      }
    }
  }
}
