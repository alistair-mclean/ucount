using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendObjectVelocityMagnitude : MonoBehaviour {
  // DESCRIPTION - Sends the magnitude of the velocity (rounded to the
  // nearest whole number) to the assigned Velocity Magnitude
  // Textbox in the External Display Canvas.

  private Text _textField;
  private bool _hasActiveObject = false; // No active object until user clicks on one.
  private Rigidbody _activeObject;

  private void Start()
  {
    _textField = GetComponent<Text>();
  }

  void Update () {
    if (Input.touchCount > 0)
    {
      Ray fingerRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
      RaycastHit hit;
      if (Physics.Raycast(fingerRay, out hit))
      {
        _activeObject = hit.rigidbody;
        _hasActiveObject = true;
      }
    }
    if (_hasActiveObject) {
      float valueToDisplay = Mathf.Round(_activeObject.velocity.magnitude);
      _textField.text = valueToDisplay.ToString();
    }
  }
}
