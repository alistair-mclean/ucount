// DESCRIPTION - This class controls the prefab to display the liquid's volume. 
// 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiquidVolumeDisplay : MonoBehaviour {
  
  // Public
  public GameObject Liquid;
  public Text VolumeText;

  // Private 
  private float _liquidVolume;
  private string _currentText = " L";

  void Start () {
    if (Liquid.GetComponent<Liquid>() == null)
    {
      Debug.LogError("No liquid component for volume display to read from!");
    }
    else 
      _liquidVolume = Liquid.GetComponent<Liquid>().GetLiquidVolume();
	}
	
	void LateUpdate () {
    _liquidVolume = Liquid.GetComponent<Liquid>().GetLiquidVolume() * 1000;
    _currentText = _liquidVolume.ToString("F0") + " Liters";
    VolumeText.text = _currentText;
  }
}
