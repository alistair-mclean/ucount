using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class singlePendulumSetup : MonoBehaviour {
  //Description : The script to control a single pendulum system. 


  //Public
  public GameObject pendulumBase;
  public GameObject pendulumString;
  public Rigidbody pendulumMass;
  public float forceModifer = 2.0f;


  //Private 
  private float _scale = 1.0f;
  private float _length = 1.0f;
  private float _mass = 0.0f;
  private float _momentum = 0.0f;
  private double _kineticEnergy = 0.0f;
  private double _potentialEnergy = 0.0f;
  private Vector3 _radius = new Vector3(0.0f, 1.0f, 0.0f); 
  private Vector3 _acceleration = new Vector3(0.0f, 0.0f, 0.0f);
  
  void Start() {
    pendulumMass.transform.position = pendulumBase.transform.position - _radius;
    _mass = pendulumMass.mass;
  }

  void FixedUpdate () {
    //Calculate acceleration, momentum, KE, and PE 
    Vector3 LastVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    _acceleration = (pendulumMass.velocity - LastVelocity) / Time.deltaTime;
    _momentum = pendulumMass.velocity.magnitude * _mass; // in kg*m/s
    _kineticEnergy = 0.5f * _momentum * pendulumMass.velocity.magnitude; // in Joules 
    _potentialEnergy = _mass * 9.81f * pendulumMass.transform.position.y;

    //Calculate and apply tension
    Vector3 tensionDir = (pendulumBase.transform.position - pendulumMass.transform.position).normalized;
    Vector3 Tension = _mass * Physics.gravity.magnitude * _scale * tensionDir;
    pendulumMass.AddForce(Tension);
    _radius = pendulumMass.position - pendulumBase.transform.position;
    

    //Vector3 Position = pendulumMass.transform.position;
    //Position = Vector3.ClampMagnitude(Position, _radius.magnitude);


    Debug.Log("mass = " + pendulumMass.mass + ", acceleration: " + _acceleration + ", Tension:" + Tension.magnitude);
  }
    
}
