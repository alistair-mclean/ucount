using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetLiquidLevel : MonoBehaviour {
  public Slider LiquidLevelSlider;
  public float TimeConstant = 0.01f; 

  private float _lastValue = 0.0f;
  private Vector3 _initialDimensions;
  private float _initialVolume;
  private float _volume;
  private List<Buoyancy> _submergedObjectsList; 

  private void Start()
  {
    _submergedObjectsList = new List<Buoyancy>();
    _initialDimensions = transform.localScale;
    _volume = _initialVolume = transform.localScale.x * transform.localScale.y * transform.localScale.z;
  }

  private void Update()
  {

  }


  private void LateUpdate()
  {
    float liquidHeight = CalculateLiquidLevel();
    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, liquidHeight);
    
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.GetComponent<Buoyancy>() == null)
    {
      Debug.LogError("No Buoyancy to object triggering inside liquid.");
    }
    Buoyancy buoyantObject = other.GetComponent<Buoyancy>();
    if (!_submergedObjectsList.Contains(buoyantObject))
      _submergedObjectsList.Add(buoyantObject);
  }

  private void OnTriggerStay(Collider other)
  {
    if (other.GetComponent<Buoyancy>() == null)
    {
      Debug.LogError("No Buoyancy to object triggering inside liquid.");
    }

    Buoyancy buoyantObject = other.GetComponent<Buoyancy>();
    if (!_submergedObjectsList.Contains(buoyantObject))
      _submergedObjectsList.Add(buoyantObject);
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.GetComponent<Buoyancy>() == null)
    {
      Debug.LogError("No Buoyancy to object triggering inside liquid.");
    }
    Buoyancy buoyantObject = other.GetComponent<Buoyancy>();
    if (_submergedObjectsList.Contains(buoyantObject))
      _submergedObjectsList.Remove(buoyantObject);
  }
  

  private float CalculateLiquidLevel()
  {
    float newLevel = _initialDimensions.z; // z because of Blender's axis being different. 
    float netSubmergedVolume = 0.0f;
    int numberOfObjects = 0;
    foreach (var buoyantObject in _submergedObjectsList)
    {

      netSubmergedVolume += buoyantObject.GetSubmergedVolume();
      numberOfObjects++;
      Debug.Log("total submerged objects: " + numberOfObjects);
    }
    float netVolume = _initialVolume + netSubmergedVolume;
    float newHeight = netVolume / (_initialDimensions.x * _initialDimensions.y);
    if (numberOfObjects == 0)
      newHeight = _initialDimensions.z;
    return newHeight;
  }

}
