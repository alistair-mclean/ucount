///<summary>
/// ReadLiquidDensity.cs - Reads the liquid density to the user.
/// 
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadLiquidDensity : MonoBehaviour {
  //  Public
  public GameObject Liquid;

  // Private
  private Liquid _liquid;

	// Use this for initialization
	void Start () {
    if (_liquid.GetComponent<Liquid>() == null)
    {
      Debug.LogError("No liquid attached to read!");
    }
    _liquid = Liquid.GetComponent<Liquid>();
    gameObject.GetComponent<Text>().text = "Liquid Density = " + _liquid.Density + "kg/m^3";
  }
	
	// Update is called once per frame
	void Update ()
  {
    gameObject.GetComponent<Text>().text = "Liquid Density = " + _liquid.Density + "kg/m^3";
  }
}
