using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserControls : MonoBehaviour {
  // DESCRIPTION - Basic user controls 

  // Public
  public GameObject DefaultObject;
  public Text ObjectIDTextBox;
  public Slider Slider1;
  public Text Slider1Text;
  public Slider Slider2;
  public Text Slider2Text;

  // Private
  private GameObject _camera;
  private Rigidbody _activeObject;
  private bool _hasActiveObject = false;

  private void Start()
  {
    if (Slider1 || Slider2) { 
      Slider1Text.text = "";
      Slider1.gameObject.SetActive(false);
      Slider2Text.text = "";
      Slider2.gameObject.SetActive(false);
    }
  }

  private void Update()
  {
    if (Input.touchCount > 0)    {
      Ray fingerRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
      RaycastHit hit;
      Vector3 distanceFromCamera = new Vector3(0.0f, 0.0f, 0.0f);
      if (Physics.Raycast(fingerRay, out hit))
      {
        _activeObject = hit.rigidbody;
        UpdateUI();
      }

      //if (Input.GetTouch(0).phase != TouchPhase.Ended)
      //{
      //  _activeObject.position = Camera.main.transform.position + distanceFromCamera;
      //}
    }

//Load the main menu if the back button is pressewd in - ANDROID ONLY
#if UNITY_ANDROID
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      Application.LoadLevel(0); //Depreciated... what to use instead?
    }
#endif
  }
  private void UpdateUI()
  {
    ObjectIDTextBox.text = "Active Object: " + _activeObject.name;
    if(_activeObject.GetComponent<Rigidbody>())
    {
      Slider1.gameObject.SetActive(true);
      Slider1Text.text = "Mass";
      Slider1.value = _activeObject.mass;
      Slider1.minValue = _activeObject.mass * 0.01f;
      Slider1.maxValue = _activeObject.mass * 10.0f;
    }
    if(_activeObject.GetComponent<PhysicMaterial>())
    {
      Debug.Log("This has some physics stuff!!");
      PhysicMaterial physicsMaterial = _activeObject.GetComponent<PhysicMaterial>();
      if(physicsMaterial.bounciness > 0)
      {
        Slider2.gameObject.SetActive(true);
        Slider2Text.text = "Bounciness";
        Slider2.value = physicsMaterial.bounciness;
        Slider2.minValue = 0;
        Slider2.maxValue = 1;
      }
      else
      {
        Slider2.gameObject.SetActive(true);
        Slider2Text.text = "Kinetic Friction";
        Slider2.value = physicsMaterial.dynamicFriction;
        Slider2.minValue = 0;
        Slider2.maxValue = 2 * physicsMaterial.dynamicFriction;
      }
    }
  }

 
}
