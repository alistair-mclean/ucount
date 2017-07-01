using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class singlePendulumSetup : MonoBehaviour {
  //Description : The script to control a single pendulum system. 

  //Private 
  private Vector2 _startPosition;
  private Vector2 _direction;
  private bool _directionChosen = false;
  //Public
  public Rigidbody pendulumBase;
  public Rigidbody pendulumMass;
  public float forceModifer = 2.0f;

  private void Start()
  {
    pendulumMass.AddForce(new Vector3(230, 0, 0));
  }

  void Update () {
    if (Input.touchCount > 0)
    {
      Touch touch = Input.GetTouch(0);
      Ray fingerRay = Camera.main.ScreenPointToRay(touch.position);
      Vector3 cameraPos = Camera.main.transform.position;
      TouchTranslation(touch);
      RaycastHit hit;
      if (Physics.Raycast(fingerRay, out hit))
      {
        if (hit.rigidbody == pendulumMass)
        {
          if (_directionChosen)
          {
            Vector3 horizontalTranslation = Camera.main.transform.right * Input.GetAxis("Horizontal");
            Vector3 verticalTranslation = Camera.main.transform.up * Input.GetAxis("Vertical");
            pendulumMass.transform.Translate(horizontalTranslation + verticalTranslation);
          }
        }
      }
    }
  }

  private void TouchTranslation(Touch thisTouch)
  { 
    TouchPhase phase = thisTouch.phase; // Handle finger movements based on touch phase.
    switch (thisTouch.phase)
    {
      // Record initial touch position.
      case TouchPhase.Began:
        _startPosition = thisTouch.position;
        _directionChosen = false;
        break;

      // Determine direction by comparing the current touch position with the initial one.
      case TouchPhase.Moved:
        _direction = thisTouch.position - _startPosition;
        break;

      // Report that a direction has been chosen when the finger is lifted.
      case TouchPhase.Ended:
        _directionChosen = true;
        break;
    }
  }
}


