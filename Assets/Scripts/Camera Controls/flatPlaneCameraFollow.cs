using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Description: camerafollowFixedAngle.cs
// Camera follow target at a fixed angle, no rotation.
// W: MUST BE USED ON A RIGIDBODY
// - Follows a target, given a specific orientation

public class flatPlaneCameraFollow : MonoBehaviour {
//PUBLIC:
    public GameObject target;


//PRIVATE;
    private Vector3 _offset;

	void Start () {
        _offset = transform.position - target.transform.position;
	}
	
	void Update () {
        transform.position = target.transform.position + _offset;
	}
}
