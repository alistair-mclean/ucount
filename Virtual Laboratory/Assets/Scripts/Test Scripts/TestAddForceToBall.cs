using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAddForceToBall : MonoBehaviour {


  private Rigidbody _thisObject;

	// Use this for initialization
	void Start () {
    _thisObject = GetComponent<Rigidbody>();
	}
	

	// Update is called once per frame
	void Update () {
	  if (Input.GetKey(KeyCode.A))
    {
      Transform TestOrigin = _thisObject.transform;
      Vector3 TestComponents = new Vector3(0.0f, 5.0f, 0.0f);
      Force newForce = new Force("Test", TestComponents, TestOrigin);
      _thisObject.AddForce(TestComponents);
      string message = "New force: " + newForce.GetForceName();
      Debug.Log(message);
    }	
	}
}
