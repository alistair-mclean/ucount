// DESCRIPTION - Resets the gameobect's position if its transform goes out of bounds.
// With the ability to toggle the resetting of forces or not. Defaulted to yes. 
// As a reminder, the y-direction is considered to be the vertical direction in all scenes!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour {

  //Public
  public bool ResetVelocity = true;
  public float MinimumHeight = -1.0f;

  //Private
  private Rigidbody _object;
  private Vector3 _initialPosition = new Vector3(0.0f, 0.0f, 0.0f);
  private Vector3 _initialVelocity = new Vector3(0.0f, 0.0f, 0.0f);
  private Bounds _boundaries;

  private void Start()
  {
//    _boundaries = ResetBounds.GetComponent<Bounds>();
    _object = GetComponent<Rigidbody>();
    _initialPosition = transform.position;
  }
  
  void LateUpdate ()
  {
    //Reset if y is outside of bounds
    if (transform.position.y <= MinimumHeight)
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
