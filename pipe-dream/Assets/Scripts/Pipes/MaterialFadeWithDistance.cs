using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MaterialFadeWithDistance : MonoBehaviour
{
  public float MaxVisibleDistance = 10.0f;

  private Transform _camera;
  private Vector3 _distance;
  private float _alphaValue;
  private Material _material;

  // Use this for initialization
  void Start()
  {
    _camera = Camera.main.transform;
    _material = gameObject.GetComponent<Renderer>().material;
  }

  // Update is called once per frame
  void Update()
  {
    _distance = _camera.transform.position - transform.position;
    float distance = _distance.sqrMagnitude;
    if (distance < MaxVisibleDistance)
      distance = MaxVisibleDistance;
    _material.color = new Vector4(_material.color.r, _material.color.g, _material.color.b, MaxVisibleDistance / distance);

  }
}
