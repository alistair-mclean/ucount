using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetObjectID : MonoBehaviour {
// DESCRIPTION - Assigns the Object Id name to the top of the main canvas 
// when the object is pressed/clicked on. 
  private Text _textfield;

  private void Start()
  {
    _textfield = GetComponent<Text>();
  }


  void Update () {
		if (Input.touchCount > 0)
    {
      Ray fingerRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
      RaycastHit hit;
      if (Physics.Raycast(fingerRay, out hit)) {
        Debug.Log(hit.rigidbody.name);
        _textfield.text = "Active Object: " + hit.rigidbody.name;
      }
    }
	}
}
