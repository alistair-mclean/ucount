using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Pendulum : MonoBehaviour {
  //Public
  public GameObject PendulumMass;
  public GameObject PendulumJoint;
  public Slider PotentialEnergySlider;
  public Slider KineticEnergySlider;
  public float PendulumLength;
  public float PendulumMassRadius;
  public float AccelerationConstant = 9.81f; // Default: Earth's gravity

  //Private
  private float _totalEnergy { get; set; }
  private float _potentialEnergy { get; set; }
  private float _kineticEnergy { get; set; }
  private float _pendulumLength { get; set; }
  private float _momentum { get; set; }

  private void Update()
  {
    if (!PendulumMass.GetComponent<Rigidbody>())
      return;

  }


  private void CalculateKineticEnergy()
  {
    float mass = PendulumMass.GetComponent<Rigidbody>().mass;
    float velocityMagnitude = PendulumMass.GetComponent<Rigidbody>().velocity.magnitude;
    float angularVelocityMagnitude = PendulumMass.GetComponent<Rigidbody>().angularVelocity.magnitude;
    float translationalKineticEnergy = (1f / 2f) * mass * Mathf.Pow(velocityMagnitude, 2f); //Translational + rotational
    float rotationalKineticEnergy = (1f / 2f) * mass * Mathf.Pow(PendulumMassRadius, 2f); //Translational + rotational

    _kineticEnergy = translationalKineticEnergy + rotationalKineticEnergy;
  }


  private void CalculatePotentialEnergy()
  {
    float mass = PendulumMass.GetComponent<Rigidbody>().mass;
  }

}
