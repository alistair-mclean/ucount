
// DESCRIPTION - This class controls object manipulation. 
// Temporarliy writing it here to not completely jumble the UserControls.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
  //Public

  //Private
  private Vector3 _relativeObjectDistance = Vector3.zero; //The relative distance from the camera position;
  private float _magnitude = 0.0f;
  private bool _objectSelected = false;
  private Vector2 _lastTouchPosition = new Vector2();
  private Vector3 _cameraPosition = new Vector3();

  void Update() {
    Camera camera = Camera.main;
    if (Input.touchCount > 0)
    {
      Ray fingerRay = camera.ScreenPointToRay(Input.GetTouch(0).position);
      RaycastHit hit;
      if (Physics.Raycast(fingerRay, out hit))
      {
        Rigidbody touchedObject = hit.rigidbody;
        Touch touch = Input.GetTouch(0);
        Vector2 touchPosition = touch.position;
        if (touchedObject.tag == "Interactable")
        {

          if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved)
          {
            //start hotfix
            //if (touchedObject.GetComponent<Buoyancy>() != null)
            //{
            //  touchedObject.GetComponent<Buoyancy>().BuoyancyIsActive = false;
            //}
            //touchedObject.useGravity = false;
            //end hotfix

            Debug.Log("Touched object = " + touchedObject.name);
            float clampMagnitude = (touchedObject.transform.position - Camera.main.transform.position).magnitude;
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector2 deltaTouchPos = touchPosition - _lastTouchPosition;

            // If the finger has moved on screen, translate the object relative to the local coordinates
            if (deltaTouchPos.x < 0)
            {
              touchedObject.transform.Translate(Vector3.left * 5 * Time.deltaTime);
            }
            else if (deltaTouchPos.x > 0)
            {
              touchedObject.transform.Translate(Vector3.right * 5 * Time.deltaTime);
            }
            if (deltaTouchPos.y < 0)
            {
              touchedObject.transform.Translate(Vector3.down * 5 * Time.deltaTime);
            }
            else if (deltaTouchPos.y > 0)
            {
              touchedObject.transform.Translate(Vector3.up * 5 * Time.deltaTime);
            }
            // Move the object relative to the camera as the camera moves around 
            Vector3 deltaCameraPos = camera.transform.position - _cameraPosition;
            touchedObject.transform.position = touchedObject.transform.position + deltaCameraPos;
          }

          //if (Input.GetTouch(0).phase == TouchPhase.Ended)
          //{
          //  if (touchedObject.GetComponent<Buoyancy>() != null)
          //  {
          //    touchedObject.GetComponent<Buoyancy>().BuoyancyIsActive = true;
          //  }
          //  touchedObject.useGravity = true;
          //}
        }
        _lastTouchPosition = touchPosition;
      }
    }
    _cameraPosition = camera.transform.position;
  }


}

  

