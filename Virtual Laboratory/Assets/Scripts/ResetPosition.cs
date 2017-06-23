using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour {
  // DESCRIPTION - Resets the gameobject to the original location 
  // once it has gone below a certain height. With the ability to
  // toggle the resetting of forces or not. Defaulted to yes. 
  // As a reminder, the y-direction is considered to be the vertical direction in all scenes!

  //Public
  public Vector3 MinimumHeight;
  public bool ResetVelocity = true;

  //Private
  private Rigidbody _object;
  private Vector3 _initialPosition = new Vector3(0.0f, 0.0f, 0.0f);

  private void Start()
  {
    _object = GetComponent<Rigidbody>();
    _initialPosition = transform.position;
  }

  void LateUpdate () {
    if (transform.position.y <= MinimumHeight.y) { 
      transform.position = _initialPosition;
      if (ResetVelocity) { 
        _object.velocity = Vector3.zero;
        _object.angularVelocity = Vector3.zero;
      }
    }
  }
}
