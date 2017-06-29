using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserControls : MonoBehaviour {

  // I need to make this script activate the ball object once clicked, and 
  // start sending information to the display canvas
  public GameObject DefaultObject;

  private GameObject _camera;
  private Rigidbody _activeObject;
 
  private void Update()
  {
    
    if (Input.touchCount > 0)    {
      Ray fingerRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
      RaycastHit hit;
      Vector3 distanceFromCamera = new Vector3(0.0f, 0.0f, 0.0f);
      if (Physics.Raycast(fingerRay, out hit))
      {
        _activeObject = hit.rigidbody;
        distanceFromCamera = _activeObject.transform.position - Camera.main.transform.position;
      }

      if (Input.GetTouch(0).phase != TouchPhase.Ended)
      {
        _activeObject.position = Camera.main.transform.position + distanceFromCamera;
      }
    }

    //Load the main menu if the back button is pressewd in - ANDROID ONLY
//#if UNITY_ANDROID
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      Application.LoadLevel(0);
    }
//#endif
  }

  //
}
