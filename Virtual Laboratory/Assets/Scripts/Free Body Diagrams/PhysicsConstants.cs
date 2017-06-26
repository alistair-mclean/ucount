using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsConstants : MonoBehaviour {
  //DESCRIPTION - This is a container class for all of the 
  // commonly used physics constants. 
  
  public double GravitationalConstant = 6.678 * Mathf.Pow(10, -11); // in kg*m/(s^2)
  public float EarthAcceleration = 9.81f; //in m/s^2
  public long SpeedOfLight = 299792458; //m/s
}
