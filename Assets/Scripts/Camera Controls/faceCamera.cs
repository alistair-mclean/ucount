using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A script for forcing sprites to face the camera
// - Makes the object look at the main camera

public class faceCamera : MonoBehaviour {
    void Update () {
        transform.LookAt(Camera.main.transform);
	}
}
