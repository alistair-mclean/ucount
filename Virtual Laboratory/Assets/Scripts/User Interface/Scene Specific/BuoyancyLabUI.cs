///<summary>
/// BuoyancyLabUI - Scene specific UI control for the Buoyancy Lab
///</summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuoyancyLabUI : MonoBehaviour {
  // Public :
  public GameObject EnvironmentLiquid;
  public Text LiquidDensityText;
  public GameObject ObjectPanel;
  public Slider LiquidDensitySlider;
  public Text ObjectIDText;
  public InputField MassInputField;

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
    if(_objectSelected)
      _activeObject.mass = float.Parse(MassInputField.text);
  }

  private void SaveUI()
  {
    if (_objectSelected)
    {
      ObjectPanel.SetActive(true);
      MassInputField.text = _activeObject.mass.ToString();
      ObjectIDText.text = _activeObject.name;
    }
    float liquidDensity = _liquid.Density;
    LiquidDensityText.text = "Density = " + liquidDensity.ToString("N1") + " kg/m^3";

  }

}
