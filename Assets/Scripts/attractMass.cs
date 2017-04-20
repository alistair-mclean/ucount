using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attracts massive objects to this objects center
public class attractMass : MonoBehaviour {
    public GameObject[] gameObjList;
    
    public float forceConstat = 200.0f;
    private Vector3 origin;
    void Start () {
        origin = transform.position; // This objects position, the local origin
    }

    void FixedUpdate()
    {
        gameObjList = GameObject.FindGameObjectsWithTag("Massive");
        foreach (GameObject gameObj in gameObjList)
        {
            float mass1 = gameObj.GetComponent<Rigidbody>().mass;
            float mass2 = GetComponent<Rigidbody>().mass;
            Vector3 r = gameObj.transform.position - origin;
            float rMagnitude = r.magnitude;
            Vector3 unit_r = r.normalized;
            Vector3 gravityForce = (forceConstat*mass1*mass1)/(rMagnitude*rMagnitude)*unit_r;
            gameObj.GetComponent<Rigidbody>().AddForce(-gravityForce);
        }
    }
}
