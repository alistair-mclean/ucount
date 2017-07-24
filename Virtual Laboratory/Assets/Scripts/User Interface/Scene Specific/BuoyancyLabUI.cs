///<summary>
/// BuoyancyLabUI - Scene specific UI control for the Buoyancy Lab
/// 
/// Copyright - VARIAL Studios LLC
///</summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuoyancyLabUI : MonoBehaviour {
  // Public :
  public GameObject EnvironmentLiquid;
  public Text LiquidDensityText;
  public Slider LiquidDensitySlider;
  public Text ObjectIDText;
  public GameObject ObjectPanel;
  public Text ObjectMassText;
  public Text ObjectDensityText;
  public Slider ObjectDensitySlider;

  // Private :
  private Liquid _liquid;
  private Rigidbody _activeObject;
  private bool _objectSelected = false;

	// Use this for initialization
	void Start () {
    ObjectPanel.SetActive(false);
    _liquid = EnvironmentLiquid.GetComponent<Liquid>();
    if (_liquid == null)
    {
      Debug.LogError("Error in BuoyancyLabUI: No Liquid component for liquid gameobject");
    }
    float liquidDensity = _liquid.Density;
    LiquidDensitySlider.minValue = liquidDensity/ 2;
    LiquidDensitySlider.maxValue = 2 * liquidDensity;
    LiquidDensitySlider.value = liquidDensity;
    LiquidDensityText.text = "Liquid Density = " + liquidDensity.ToString();
  }

  private void LateUpdate()
  {
    if (Input.touchCount > 0)
    {
      Ray fingerRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
      RaycastHit hit;
      if (Physics.Raycast(fingerRay, out hit))
      {
        if (hit.collider.tag == "Interactable")
          _activeObject = hit.rigidbody;
          _objectSelected = true;
      }
    }
    SaveUI();
    _liquid.SetLiquidDensity(LiquidDensitySlider.value);
  }

  private void SaveUI()
  {
    if (_objectSelected)
    {
      ObjectPanel.SetActive(true);
      ObjectMassText.text = "Mass = " + _activeObject.mass.ToString() + " kg";
      ObjectIDText.text = _activeObject.name;
      float objectDensity = _activeObject.GetComponent<Buoyancy>().GetObjectDensity();
      ObjectDensityText.text = "Density = " + objectDensity.ToString("N1") + " kg/m^3";
      ObjectDensitySlider.minValue = objectDensity / 2f;
      ObjectDensitySlider.maxValue = objectDensity * 1.5f;
      ObjectDensitySlider.value = objectDensity;
    }
    float liquidDensity = _liquid.Density;
    LiquidDensityText.text = "Density = " + liquidDensity.ToString("N1") + " kg/m^3";

  }

}
