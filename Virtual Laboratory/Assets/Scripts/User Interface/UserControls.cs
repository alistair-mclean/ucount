using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserControls : MonoBehaviour {

  // I need to make this script activate the ball object once clicked, and 
  // start sending information to the display canvas
  public GameObject DefaultObject;

  private GameObject _camera;
  private GameObject _activeObject;
 
  private void Update()
  {
    
    if (Input.touchCount > 0)    {
      Ray fingerRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
      RaycastHit hit;
      if (Physics.Raycast(fingerRay, out hit))
      {
        Rigidbody selectedObject = hit.rigidbody;
      }
      
      if (Input.GetTouch(0).phase == TouchPhase.Moved)
      {
        // Do some stuff if the finger has moved
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

  private void ActivateObject(Rigidbody obj)
  {
    if (obj.tag == "Interactable")
    {
      if (obj.gameObject.activeSelf)
      {
        return;
      }
      else
     {
        obj.gameObject.SetActive(true); // this wont work for obvious reasons... 
      }
    }
    

  }
  //
}
