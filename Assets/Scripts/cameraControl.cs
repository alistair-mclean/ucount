using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour {
    public GameObject reference; // The center of the level
    public GameObject target;


    private Vector3 radialOffset;
    private float localAngle;
    private float constant = 0.4f;

    void Start () {

    }

    void LateUpdate () {
        radialOffset = (target.transform.position - reference.transform.position);
        transform.position = target.transform.position + radialOffset;
        Quaternion rotation = Quaternion.LookRotation(-radialOffset);
        string message = "radialOffset(" + radialOffset.x + ", " + radialOffset.y + ", " + radialOffset.z + ")";
        Debug.Log(message);
        transform.rotation = rotation;
    }
}
