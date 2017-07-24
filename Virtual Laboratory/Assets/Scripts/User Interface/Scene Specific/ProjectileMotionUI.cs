///<summary>
/// ProjectileMotionUI - Scene specific user interface control for the projectile motion lab
/// 
/// Copyright - VARIAL Studios LLC 
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ProjectileMotionUI : MonoBehaviour {
  //Public
  public Text ObjectID;
  public InputField MassInput;
  public Dropdown MassUnitsDropdown;
  public InputField VelocityX;
  public InputField VelocityY;
  public InputField VelocityZ;
  public Dropdown VelocityDistanceUnits;
  public Dropdown VelocityTimeUnits;
  public InputField AngleInput;
  public Dropdown AngleUnits;

  //Private
  private Rigidbody _selectedObject;
  private float _selectedMass = 0.0f;
  private float _angleTheta = 0.0f;
  private bool _objectSelected = false;
  private Vector3 _initialVelocity = Vector3.zero;
  private Vector3 _initialPosition;
  private enum MassUnits { kg, lb};
  private enum VelocityDistanceScale { meter, feet, mile };
  private enum TimeRate { second, minute, hour};
  private enum angleUnits { degrees, radians };

  private MassUnits _massUnits = MassUnits.kg; //default units
  private VelocityDistanceScale _velocityDistanceScale = VelocityDistanceScale.meter;
  private TimeRate _velocityTimeRate = TimeRate.second;
  private angleUnits _angleUnits = angleUnits.degrees;
  

  void Update() {
    if (Input.touchCount > 0) {
      Ray fingerRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
      RaycastHit hit;
      if (Physics.Raycast(fingerRay, out hit))
      {
        Rigidbody obj = hit.rigidbody;
        if (obj.tag == "Interactable")
        {
          _selectedObject = obj;}
          _initialPosition = _selectedObject.transform.position;
          _objectSelected = true;
        }
      }
    
    if (_objectSelected) { 
      SetInterface();
    }
  }

  private void SetInterface()
  {
    if (_selectedObject)
    {
      ObjectID.text = _selectedObject.name;
      _selectedMass = _selectedObject.mass;
      float massUnitScale = 1.0f; // Kg
      if (_massUnits != MassUnits.kg)
        massUnitScale = 2.20462f; //scale kg to lbs 
      _selectedMass = float.Parse(MassInput.text) * massUnitScale;
      MassInput.text = _selectedMass.ToString();
    }
  }

  private void SaveUI()
  {
    _selectedMass = _selectedObject.mass;
    float initialVelocityX = float.Parse(VelocityX.text);
    float initialVelocityY = float.Parse(VelocityY.text);
    float initialVelocityZ = float.Parse(VelocityZ.text);
    _initialVelocity = new Vector3(initialVelocityX, initialVelocityY, initialVelocityZ);
    if (AngleInput.text != null)
      _angleTheta = float.Parse(AngleInput.text);
    else
      _angleTheta = 0.0f;
  }


  public void LaunchProjectile()
  {
    if (_objectSelected)
    {    
      _selectedObject.AddForce(_initialVelocity);
    }
  }
}

