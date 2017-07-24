using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrictionRamp : MonoBehaviour {

  public float RampGrowthTimeConstant = 1.0f;

  private Vector3 _initialDimensions;
  private Vector3 _newDimensions;
  private bool _hasTouched = false;

	void Start () {
    _initialDimensions = transform.localScale;
    _newDimensions = _initialDimensions;
  }

  void Update()
  {
    if (!_hasTouched)
    {
      _newDimensions.x += 0.05f*_newDimensions.x; // Increase the size by 10% each increment
      transform.localScale = Vector3.Lerp(_initialDimensions, _newDimensions, RampGrowthTimeConstant * Time.deltaTime);
    }
  }

  private void OnCollisionEnter(Collision collision)
  {

    Debug.Log("oncollisionenter with " + collision.gameObject.name + " with " + gameObject.name);
  }
  private void OnCollisionExit(Collision collision)
  {
    Debug.Log("oncollisionexit with " + collision.gameObject.name + " with " + gameObject.name);
  }

  private void OnTriggerEnter(Collider other)
  {
    Debug.Log("Ontriggerenter with " + other.name + " with " + gameObject.name);
    _hasTouched = true;
  }

  private void OnTriggerExit(Collider other)
  {
    Debug.Log("Ontriggerexit with " + other.name + " with " + gameObject.name);
    _hasTouched = false;
  }
}
