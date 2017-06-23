using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowComponentAttributes : MonoBehaviour {
  // DESCRIPTION - The script to 'activate' the information for an object
  // and apply it to the correct Display Canvas. 
  //

  //Public 
  public GameObject VectorComponents;
  public bool DisplayVelocity = true;
  public bool DisplayAcceleration = false;
  public bool DisplayNetForce = false;

  //Private
  private Rigidbody _thisObject;
  private Vector3 _acceleration = new Vector3(0.0f, 0.0f, 0.0f);
  private Vector3 _momentum = new Vector3(0.0f, 0.0f, 0.0f);
  private Vector3 _lastVelocity = Vector3.zero;
  private float _kineticEnergy = 0.0f;
  private float _potentialEnergy = 0.0f;
  private float _earthAcceleration = 9.81f;

  void Start()
  {
    _thisObject = GetComponent<Rigidbody>();
  }

  public void CalculateComponents()
  {
    _acceleration = (_thisObject.velocity - _lastVelocity) / Time.deltaTime;
    _momentum = _thisObject.mass * _thisObject.velocity;
    _kineticEnergy = (1 / 2) * _thisObject.mass * Mathf.Pow(_thisObject.velocity.magnitude, 2.0f); // KE = (1/2)mv^2
    _potentialEnergy = _thisObject.mass * _earthAcceleration * _thisObject.position.y; // PE = mgh 

  }

  void LateUpdate () {
    CalculateComponents();   
    _lastVelocity = _thisObject.velocity;
  }

  public float GetVelocityMagnitude()
  {
    return _thisObject.velocity.magnitude;
  }

  public float GetAccelerationMagnitude()
  {
    return _acceleration.magnitude;
  }
}
