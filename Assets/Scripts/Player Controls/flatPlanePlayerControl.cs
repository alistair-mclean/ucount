using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Description: flatPlanePlayerControl.cs
// A script to control player movement on a flat plane 
// I: TO BE USED WITH A FIXED ANGLE CAMERA
// - Adds a force to the player based on horizontal or vertical input
// - Adds a speed boost if shift is pressed 
// - Increases the player size as they collide with gameobjects tagged as Grass
//    - Decreases the player size while shift is pressed
// 

public class flatPlanePlayerControl : MonoBehaviour {
    //PUBLIC:
    public float speed = 200.0f;
    public float boost = 3.0f;
    public float growthConstant = 1.4f;
    public float shittyConstant = 0.1f;
    public float smoothTimeConstant = 0.2f;

    //PRIVATE:
    private Rigidbody _rb;
    private Vector3 _scale;
    private Vector3 _velocity;
    private int _numEaten;
    private int _totalNumEaten;
    private float _horizontalAxisInput;
    private float _verticalAxisInput;
    private float _maxSpeed = 13.0f;


    void Start() {
        _numEaten = 0;
        _totalNumEaten = 0;
        _rb = GetComponent<Rigidbody>();
        _scale = _rb.transform.localScale;
    }

    void Update() {
        _horizontalAxisInput = Input.GetAxis("Horizontal");
        _verticalAxisInput = Input.GetAxis("Vertical");

        // Limit the throttle input from the user 
        if (_rb.velocity.magnitude <= _maxSpeed) {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) {
                _rb.AddForce(_horizontalAxisInput * speed * boost, 0.0f, _verticalAxisInput * speed * boost);
                shrinkPlayer();
            }
            _rb.AddForce(_horizontalAxisInput * speed, 0.0f, _verticalAxisInput * speed);
        }
        if (Input.GetKeyDown(KeyCode.G))
            growPlayer();
        if (Input.GetKeyDown(KeyCode.S))
            shrinkPlayer();
    }

    void LateUpdate() {
        _velocity = _rb.velocity;
    }
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Grass")
        {
            _numEaten++;
            _totalNumEaten++;
            growPlayer();
            other.gameObject.SetActive(false);
        }
    }

    void growPlayer() {
        // Need to add a limit!!
        Vector3 newScale = _rb.transform.localScale + _scale * growthConstant;
        Debug.Log("Growth scale: " + newScale.magnitude);
        _rb.transform.localScale = Vector3.Lerp(_rb.transform.localScale, newScale, smoothTimeConstant);
    }

    void shrinkPlayer() {
        // Need to add a limit!!
        Vector3 newScale = _rb.transform.localScale - _scale * shittyConstant;
        Debug.Log("Shrink scale: " + newScale.magnitude);
        if (newScale.magnitude > _scale.magnitude * 0.1f) { 
            _rb.transform.localScale = Vector3.Lerp(_rb.transform.localScale, newScale, smoothTimeConstant);
        }
        else
        {
            Debug.Log(" Too small! ");
        }
    }
}
