using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour {
    public float speed = 10.0f;
    public GameObject playerCamera;


    private Rigidbody rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        float i_horizontal = Input.GetAxis("Horizontal");
        float i_vertical = Input.GetAxis("Vertical");
//        Debug.Log("horizontal input: " + i_horizontal + " vertical input: " + i_vertical);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(playerCamera.transform.up * speed);
            float magnitude = playerCamera.transform.up.magnitude;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddForce(-playerCamera.transform.right * speed);
            float magnitude = playerCamera.transform.right.magnitude;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForce(playerCamera.transform.right * speed);
            float magnitude = playerCamera.transform.right.magnitude;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            rb.AddForce(-playerCamera.transform.up * speed);
            //float magnitude = playerCamera.transform.up.magnitude;
            //Debug.Log("S playerCamera.transform.up.magnitude: " + magnitude);
        }
    }
}
