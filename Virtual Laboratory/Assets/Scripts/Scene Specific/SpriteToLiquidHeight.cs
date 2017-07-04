using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteToLiquidHeight : MonoBehaviour {
  // DESCRIPTION - This class sets the assigned sprite's height to the height of the liquid. 

  // Public
  public GameObject Liquid;

  // Private
  private float _height = 0.5f;

	void Start () {
    float newHeight = 2 * Liquid.transform.localScale.z; //BECAUSE OF BLENDER - MUST ADDRESS SOON 
    _height = newHeight;
    transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
  }
	
	void LateUpdate ()
  {
    float newHeight = 2 * Liquid.transform.localScale.z; //BECAUSE OF BLENDER - MUST ADDRESS SOON 
    _height = newHeight;
    transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
  }
}
