using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Liquid : MonoBehaviour {
  // DESCRIPTION - This script applys a buoyant force to an object that is 
  // within the mesh of this one.

  // Public
  public float LiquidDensity = 1.0f; // in kg/m^3
  public float BuoyancyForceConstant = 0.0001f;
  public float LiquidLevelRestoringConstant = 0.5f;
  public Slider UISlider;

  // Private
  private GameObject _liquid;
  private Mesh _liquidMesh;
  private Vector3 _liquidDimensions;
  private Vector3 _initialWaterDimensions;
  private float _liquidMass;
  private float _totalVolume;
  private List<Rigidbody> _submergedObjects;
  

	void Start () {
    _liquid = gameObject;
    _initialWaterDimensions = _liquid.transform.localScale;
    _liquidDimensions = _liquid.transform.localScale;
    _totalVolume = _liquidDimensions.x * _liquidDimensions.y * _liquidDimensions.z;
    _liquidMass = LiquidDensity * _totalVolume;
    _liquidMesh = _liquid.GetComponent<Mesh>();
	}

  void Update()
  {
    _liquidDimensions = _liquid.transform.localScale;
    float changeInDepth = _liquid.transform.localScale.y - _initialWaterDimensions.y;
    // Loop through all of the submerged objects and adjust the liquid level (LERP) for each.

    // Restore the liquid's height
    _liquid.transform.localScale = Vector3.Lerp(_liquid.transform.localScale, _initialWaterDimensions, changeInDepth);
    _totalVolume = _liquidDimensions.x * _liquidDimensions.y * _liquidDimensions.z;

  }

  // DEPRECIATED - SOON TO BE DELETED
  public Vector3 BuoyancyForce(GameObject newObject) {
    Mesh newObjectMesh = newObject.GetComponent<Mesh>();
    Vector3 newObjectDimensions = newObject.transform.localScale;
    float newObjectHeight = newObject.transform.position.y;
    float liquidHeightOnObject = 0.0f;
    float effectiveVolume = 0.0f;

    // If the bottom of the object is higher than the height of the water
    if (newObject.transform.position.y - newObjectDimensions.y/2 > _liquidDimensions.y)
    {
      //Check this, it might be far from correct
      liquidHeightOnObject = newObjectHeight - (newObjectHeight / 2 + (newObject.transform.position.y - _liquidDimensions.y));
    }
    if (newObject.transform.position.y < _liquidDimensions.y)
    {
      //Check this, it might be far from correct
      liquidHeightOnObject = newObjectHeight - (_liquidDimensions.y - newObjectHeight + newObjectHeight / 2);
    }
    effectiveVolume = newObjectDimensions.x * liquidHeightOnObject * newObjectDimensions.z;

    float newObjectMass = newObject.GetComponent<Rigidbody>().mass;
    Vector3 forceOfGravityOnNewObject = newObjectMass * Vector3.down * 9.81f;

    Vector3 force = effectiveVolume * LiquidDensity * forceOfGravityOnNewObject;
    Debug.Log("Mass of Object = " + newObjectMass + " kg" );
    Debug.Log("Force of gravity on Object = " + forceOfGravityOnNewObject + " N");
    Debug.Log("Buoyancy Force = (" + force.x + ", " + force.y + ", " + force.z + ") N");
    force = force * BuoyancyForceConstant;
    return force;
  }
  // SOON TO BE DELETED - end

  private void OnCollisionEnter(Collision collision)
  {
    Buoyancy collidingObjectBuoyancy = collision.gameObject.GetComponent<Buoyancy>();
    _totalVolume += collidingObjectBuoyancy.SubmergedVolume;
    float heightToAdd = _totalVolume / (_liquidDimensions.x * _liquidDimensions.y) - _initialWaterDimensions.z;
    LerpLiquidHeightToValue(heightToAdd);
  }

  private void OnCollisionStay(Collision collision)
  {
    Buoyancy collidingObjectBuoyancy = collision.gameObject.GetComponent<Buoyancy>();
    _totalVolume += collidingObjectBuoyancy.SubmergedVolume;
    float heightToAdd = _totalVolume / (_liquidDimensions.x * _liquidDimensions.y) - _initialWaterDimensions.z;
    LerpLiquidHeightToValue(heightToAdd);
  }


  public void LerpLiquidHeightToValue(float newHeight)
  {
    Vector3 newScale = _liquidDimensions;
    newScale.z += newHeight;
    transform.localScale = Vector3.Lerp(transform.localScale, newScale, newHeight);
  }
}
