using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Description: orbitalCameraControl.cs
// A script to control the cameras motion
// - Follows an object LateUpdate
// - Reorients to object location in reference to a point on LateUpdate

public class orbitalCameraControl : MonoBehaviour { 

//PUBLIC VARIABLES
    public GameObject reference; // The center of the level
    public GameObject target;

//PRIVATE VARIABLES 
    private Vector3 _radialOffset;
    private float _localAngle;
    private float _constant = 0.6f;

    void LateUpdate () {
        _radialOffset = (target.transform.position - reference.transform.position);
        transform.position = _constant*(target.transform.position + _radialOffset);
        Quaternion rotation = Quaternion.LookRotation(-_radialOffset);
        transform.rotation = rotation;
    }
}
