using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Description: attractMass.cs
//Attracts rigidbodies tagged "Massive" to this object
// - Populates a list of gameobject
// - Adds a gravitational attraction to each object targeted as massive

public class attractMass : MonoBehaviour {
//PUBLIC: 
    public GameObject[] gameObjList; 
    public float forceConstat = 200.0f;

    void FixedUpdate()
    {
        gameObjList = GameObject.FindGameObjectsWithTag("Massive");
        foreach (GameObject gameObj in gameObjList)
        {
            float mass1 = gameObj.GetComponent<Rigidbody>().mass;
            float mass2 = GetComponent<Rigidbody>().mass;
            Vector3 r = gameObj.transform.position - transform.position;
            float rMagnitude = r.magnitude;
            Vector3 unit_r = r.normalized;
            Vector3 gravityForce = (forceConstat*mass1*mass1)/(rMagnitude*rMagnitude)*unit_r;
            gameObj.GetComponent<Rigidbody>().AddForce(-gravityForce);
        }
    }
}
