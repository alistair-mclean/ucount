using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialForce : MonoBehaviour {
  // DESCRIPTION - Adds an initial Force to an object 
  public Vector3 ForceOnObject = new Vector3(0, 0, 0);

  private Rigidbody _object;
	// Use this for initialization
	void Start () {
    _object = GetComponent<Rigidbody>();
    _object.AddForce(ForceOnObject);
	}
}
