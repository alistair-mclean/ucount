
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

  Ray GenerateTouchRay()
  {
    Touch touch = Input.GetTouch(0);
    Vector3 touchPosNear = new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane);
    Vector3 touchPosFar= new Vector3(touch.position.x, touch.position.y, Camera.main.farClipPlane);

    Ray touchRay = new Ray(touchPosNear, touchPosFar - touchPosNear);

    return touchRay;
  }

  void LateUpdate() {
    Camera camera = Camera.main;
    if (Input.touchCount > 0)
    {
      //      Ray fingerRay = camera.ScreenPointToRay(Input.GetTouch(0).position);
      Ray fingerRay = GenerateTouchRay();    
      RaycastHit hit;
      if (Physics.Raycast(fingerRay, out hit))
      {
        Rigidbody touchedObject = hit.rigidbody;
        Touch touch = Input.GetTouch(0);
        Vector2 touchPosition = touch.position;
        if (touchedObject.tag == "Interactable")
        {

          if (Input.GetTouch(0).phase == TouchPhase.Began)
          {

            Debug.Log("Touched object = " + touchedObject.name);
            float clampMagnitude = (touchedObject.transform.position - Camera.main.transform.position).magnitude;
          
            // lerp and set the position of the current object to that of the touch, but smoothly over time.
          }
          if (Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(0).phase == TouchPhase.Moved)
          {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector2 deltaTouchPos = Input.GetTouch(0).deltaPosition;

            touchedObject.transform.Translate(Camera.main.transform.right * 100 * horizontalInput * Time.deltaTime);
            touchedObject.transform.Translate(Camera.main.transform.up * 100 * verticalInput * Time.deltaTime);         // get the touch position from the screen touch to world point
            Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touchedObject.transform.position.x, touch.position.y, touch.position.x));
            touchedObject.transform.position = Vector3.Lerp(touchedObject.transform.position, touchedPos, Time.deltaTime);


          }


          if (Input.GetTouch(0).phase == TouchPhase.Ended)
          {
            // Maybe do something????
          }
        }
        _lastTouchPosition = touchPosition;
      }
    }
    _cameraPosition = camera.transform.position;
  }


}

  

