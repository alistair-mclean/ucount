///<summary>
/// UserControls.cs - Container class for general user controls. 
/// 
/// Copyright - VARIAL Studios LLC 
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        if (hit.collider.tag == "Interactable")
          _activeObject = hit.rigidbody;
      }

      if (Slider2.IsActive())
      {
        _activeObject.GetComponent<Buoyancy>().ObjectDensity = Slider2.value;
      }
      if (Slider2.IsActive())
      {
        _activeObject.GetComponent<Buoyancy>().ObjectDensity = Slider2.value;
      }
    }

//Load the main menu if the back button is pressewd in - ANDROID ONLY
#if UNITY_ANDROID
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      SceneManager.LoadScene(0);
    }
#endif
  }
  }

 

