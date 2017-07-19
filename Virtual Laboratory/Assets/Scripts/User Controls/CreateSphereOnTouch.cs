using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSphereOnTouch : MonoBehaviour {
  // the point of this class is to create a sphere when the user pinches.
  // it will create the sphere a certain distance from the camera, based on the user's touch position. 

  public float SphereSpawnDistance = 5.0f;
  public float ReductionFactor = 0.01f;

  private GameObject _sphere;
  private bool _sphereExists = false;

	void Update () {
		if (Input.touchCount > 0)
    {
      if (Input.touchCount == 2) // may need to add a constraint in the form of a time elapsed variable
      { // if a certain amount of time has passed after the pinch has begun, and if they're a certain distance from one another
        Vector2 firstTouchPosition = Input.GetTouch(0).position;
        Vector2 secodTouchPosition = Input.GetTouch(1).position;
        
        // calculate the distance from the touches, and create a sphere based on that distance. 
        Vector2 touchDistance = secodTouchPosition - firstTouchPosition;
        float sphereRadius = touchDistance.magnitude * ReductionFactor; //perhaps combined with the product of some other number and itself. 


        Vector3 sphereSpawnPosition = new Vector3(firstTouchPosition.x, firstTouchPosition.y, SphereSpawnDistance);
        sphereSpawnPosition = Camera.main.ScreenToWorldPoint(sphereSpawnPosition);

        if (!_sphereExists)
          CreateSphere(sphereRadius, sphereSpawnPosition);
        
        //Scale the sphere with the touch distance
        _sphere.transform.localScale = new Vector3(sphereRadius / 2f, sphereRadius / 2f, sphereRadius / 2f);
      }

    }
	}

  
  void CreateSphere(float radius, Vector3 spawnPosition)
  {
    _sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    _sphere.AddComponent<Rigidbody>();
    _sphere.GetComponent<Rigidbody>().useGravity = false;

    _sphere.AddComponent<ObjectState>();
    _sphere.GetComponent<ObjectState>().SetStateIdle();

    _sphere.transform.localScale = new Vector3(radius / 2f, radius / 2f, radius / 2f);
    _sphere.transform.position = spawnPosition;
    _sphere.tag = "Interactable";
    _sphere.name = "Derp";
   // Instantiate(_sphere);
    _sphereExists = true;
  }
}
