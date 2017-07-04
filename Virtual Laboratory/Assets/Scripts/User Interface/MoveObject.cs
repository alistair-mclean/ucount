using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour {
  // DESCRIPTION - This class controls object manipulation. 
  // Temporarliy writing it here to not completely jumble the UserControls.cs

  //Public

  //Private
  private Vector3 _relativeObjectDistance = Vector3.zero; //The relative distance from the camera position;
  private float _magnitude = 0.0f;
  private bool _objectSelected = false;

  void Update () {
		if (Input.touchCount > 0 )
    {
      Ray fingerRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
      RaycastHit hit;
      if (Physics.Raycast(fingerRay, out hit))
      {
        Rigidbody touchedObject = hit.rigidbody;
        Touch touch = Input.GetTouch(0);
        if (touchedObject.tag == "Interactable")
        {
          Debug.Log("Touched object = " + touchedObject.name);
          float clampMagnitude = (touchedObject.transform.position - Camera.main.transform.position).magnitude;
          float horizontalInput = Input.GetAxis("Horizontal");
          float verticalInput = Input.GetAxis("Vertical");
          Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.transform.position.z));

          //touchedObject.transform.position = Vector3.ClampMagnitude(touchedObject.transform.position, clampMagnitude
          if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved) { 
            touchedObject.transform.Translate(Camera.main.transform.right * horizontalInput);
            touchedObject.transform.Translate(Camera.main.transform.up * verticalInput);
          }
        }
      }
    }

	}

  
}
