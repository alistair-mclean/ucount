///<summary>
/// MoveObject.cs - Controls object manipulation.
/// 
/// Copyright - VARIAL Studios LLC 
///</summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
  //Public
  public float MovementConstant = 1f;
  public float TimeConstant = 1f; 


  //Private
  private GameObject _selectedObject;
  private Vector3 _relativeObjectDistance = Vector3.zero; //The relative distance from the camera position;
  private float _magnitude = 0.0f;
  private bool _objectSelected = false;


  void LateUpdate() {

    Camera camera = Camera.main;
    if (Input.touchCount > 0)
    {
      // If the user still has their finger on the screen from touching an object in the last Update call
      // but the ray is not colliding with it; adjust the position of it properly.

      Ray fingerRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);    
      RaycastHit hit;
      if (Physics.Raycast(fingerRay, out hit))
      {
        Rigidbody touchedObject = hit.rigidbody;
        Touch touch = Input.GetTouch(0);
        Vector2 touchPosition = touch.position;

        // Only if the object is interactable do we do move it. 
        if (touchedObject.tag == "Interactable")
        {
          _selectedObject = touchedObject.gameObject;
          Vector2 initialTouchPos;
          _relativeObjectDistance = Camera.main.transform.position - _selectedObject.transform.position;

          // Touch has begun, set the object state to active. 
          if (Input.GetTouch(0).phase == TouchPhase.Began)
          {
            initialTouchPos = Input.GetTouch(0).position;
            _selectedObject.GetComponent<ObjectState>().SetStateActive();
            _objectSelected = true;

            _selectedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _selectedObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            //Vector3 initialPosition = Input.GetTouch(0).position;
            //initialPosition.z = distanceFromObject.magnitude;
            //_selectedObject.transform.position = Camera.main.ScreenToWorldPoint(initialPosition);
          }

        }
      }

      // In-transition state
      if (Input.GetTouch(0).phase == TouchPhase.Moved && _objectSelected)
      {
        Vector3 movePosition = Input.GetTouch(0).position;
        Debug.Log("Move position = (" + movePosition.x + " , " + movePosition.y + " , " + movePosition.z + ") ");
        movePosition.z = _relativeObjectDistance.magnitude;
        _selectedObject.transform.position = Vector3.Lerp(_selectedObject.transform.position, Camera.main.ScreenToWorldPoint(movePosition), TimeConstant);
      }

      // User has stopped touching, return the object to it's idle (default) state
      if (Input.GetTouch(0).phase == TouchPhase.Ended && _objectSelected)
      {
        _selectedObject.GetComponent<ObjectState>().SetStateIdle();
        _objectSelected = false;
        _selectedObject = null;
      }
    }
  }
}

  

