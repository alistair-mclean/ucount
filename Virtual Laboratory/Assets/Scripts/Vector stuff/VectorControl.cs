using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorControl : MonoBehaviour {
  //DESCRIPTION - Controls the movement and scaling for Vector prefabs
  

  private GameObject _vector;
  private Vector3 _originalScale;
  private float _scale = 1.0f; 



	void Start () {
    _vector = GetComponent<GameObject>();
    _originalScale = _vector.transform.localScale;
	}

  public void SetVectorScale(float newScale)
  {
    _vector.transform.localScale = new Vector3(newScale, newScale, newScale);
  }

  public void SetVectorScale(Vector3 newScale)
  {
    _vector.transform.localScale = newScale;
  }


  void ResetVectorScale() {
    _vector.transform.localScale = _originalScale;
  }

  void Update () {
		
	}
}
