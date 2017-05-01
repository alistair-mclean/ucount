using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Description: playerControl.cs
// A script for controlling a player ball, with a reference camera
// W: REQUIRES A PLAYER CAMERA, WILL NOT WORK PROPERLY OTHERWISE!
// I: APPLY THIS TO A RIGIDBODY
// - WASD(<-,^,v,->) Based movement
// - Adds a force to a direction relative to the player's camera's perspective
// - Pressing shift will add a speed boost multiplier

public class orbitalPlayerControl : MonoBehaviour {
//PUBLIC
    public float speed = 300.0f;
    public float boost = 3.0f; // boost multiplier when shift is pressed
    public GameObject playerCamera;

//PRIVATE
    private Rigidbody _rb;

    void Start () {
        _rb = GetComponent<Rigidbody>();
	}
	
	void Update () {

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                _rb.AddForce(playerCamera.transform.up * speed * boost);
            _rb.AddForce(playerCamera.transform.up * speed);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                _rb.AddForce(-playerCamera.transform.right * speed * boost);
            _rb.AddForce(-playerCamera.transform.right * speed);

        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                _rb.AddForce(playerCamera.transform.right * speed * boost);
            _rb.AddForce(playerCamera.transform.right * speed);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                _rb.AddForce(-playerCamera.transform.up * speed * boost);
            _rb.AddForce(-playerCamera.transform.up * speed);
        }
    }
}
