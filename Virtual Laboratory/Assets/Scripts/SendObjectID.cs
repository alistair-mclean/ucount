using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendObjectID : MonoBehaviour {
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
        _textfield.text = hit.rigidbody.name;
      }
    }
	}
}
