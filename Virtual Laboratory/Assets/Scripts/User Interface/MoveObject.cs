
// DESCRIPTION - This class controls object manipulation. 
// Temporarliy writing it here to not completely jumble the UserControls.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
  //Public
  public float MovementConstant = 10.0f;
  public float TimeConstant = 0.3f; // 


  //Private
  private GameObject _selectedObject;
  private Vector3 _relativeObjectDistance = Vector3.zero; //The relative distance from the camera position;
  private float _magnitude = 0.0f;
  private bool _objectSelected = false;


  void Update() {

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

          // Touch has begun, set the object state to active. 
          if (Input.GetTouch(0).phase == TouchPhase.Began)
          {
            initialTouchPos = Input.GetTouch(0).position;
            _selectedObject.GetComponent<ObjectState>().SetActive();
            _objectSelected = true;
          }

          // In-transition state
          if (Input.GetTouch(0).phase == TouchPhase.Moved)
          {
            Vector2 deltaTouchPos = Input.GetTouch(0).deltaPosition;
            Vector2 movedTouchPos = Input.GetTouch(0).position;
            Vector3 newPos = new Vector3(fingerRay.origin.x + deltaTouchPos.x * MovementConstant, fingerRay.origin.y + deltaTouchPos.y *MovementConstant, _selectedObject.transform.position.z);
            _selectedObject.transform.position = Vector3.Lerp(_selectedObject.transform.position, newPos, Time.deltaTime * TimeConstant);
          }

          // User has stopped touching, return the object to it's idle (default) state
          if (Input.GetTouch(0).phase == TouchPhase.Ended)
          {
            touchedObject.GetComponent<ObjectState>().SetIdle();
            _objectSelected = false;
            _selectedObject = null;
          }
        }
      }

      if (_objectSelected)
      {
        Debug.Log("Object is selected!");
        Vector2 deltaTouchPos = Input.GetTouch(0).deltaPosition;
        Vector2 movedTouchPos = Input.GetTouch(0).position;
        Vector3 newPos = new Vector3(fingerRay.origin.x + deltaTouchPos.x * MovementConstant, fingerRay.origin.y + deltaTouchPos.y * MovementConstant, _selectedObject.transform.position.z);
        _selectedObject.transform.position = Vector3.Lerp(_selectedObject.transform.position, newPos, Time.deltaTime * TimeConstant);

      }
    }
  }


}

  

