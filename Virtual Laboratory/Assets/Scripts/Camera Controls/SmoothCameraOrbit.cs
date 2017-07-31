///<summary>
/// SmoothCameraOrbit.cs - Contains controls for smooth camera 
/// orbit behavior about an object.
/// Copyright - VARIAL Studios LLC
/// </summary>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraOrbit : MonoBehaviour {
  // Public 
  public Transform Target;
  public float Distance = 10.0f;
  public float MaxDistance = 20.0f;
  public float ZoomSpeed = 2.0f;
  public float XSpeed = 250.0f;
  public float YSpeed = 120.0f;
  public float YMinLimit = -80.0f;
  public float YMaxLimit = 80.0f;
  public float SmoothTime = 0.3f;
  public float PerspectiveZoomSpeed = 0.5f;
  public float OrthographicZoomSpeed = 0.5f;
  public float LookSensitivity = 2f;
  public float RotationSpeedConstant = 1.5f;

  // Private
  private float _x = 0.0f;
  private float _y = 0.0f;
  private float _xSmooth = 0.0f;
  private float _ySmooth = 0.0f;
  private float _xVelocity = 0.0f;
  private float _yVelocity = 0.0f;
  private Vector3 _posSmooth = Vector3.zero; // WHAT IS THIS??
  private Vector3 _posVelocity = Vector3.zero;

  
  
	void Start () { 
    Vector3 angles = transform.eulerAngles;
    _x = angles.y;
    _y = angles.x;

    if (GetComponent<Rigidbody>())
      GetComponent<Rigidbody>().freezeRotation = true;

#if MOBILE_INPUT
    RotationSpeedConstant = 8.0f;
#else
    _rotationSpeedConstant = 20.0f;
#endif
  }

	
	void LateUpdate () {
    float rotationalSpeed = LookSensitivity * RotationSpeedConstant;

    if (Input.GetMouseButton(0) && Input.touchCount == 0)
    { //missing the event system to go along with this
      _x += Input.GetAxisRaw("Mouse X") * rotationalSpeed;
      _y += Input.GetAxisRaw("Mouse Y") * rotationalSpeed;
    }
    else if((Input.touchCount >= 3 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetKeyDown(KeyCode.Mouse1))
    { //also missing the event system for this 
      _x += Input.GetTouch(0).deltaPosition.x * rotationalSpeed;
      _y += Input.GetTouch(0).deltaPosition.y * rotationalSpeed;
    }
    _y = Mathf.Clamp(_y, YMinLimit, YMaxLimit);

    _xSmooth = Mathf.SmoothDamp(_xSmooth, _x, ref _xVelocity, SmoothTime);
    _ySmooth = Mathf.SmoothDamp(_ySmooth, _y, ref _yVelocity, SmoothTime);
    Quaternion rotation = Quaternion.Euler(_ySmooth, _xSmooth, 0);

    transform.rotation = rotation;

    Distance -= Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
    Distance = Mathf.Clamp(Distance, -MaxDistance, MaxDistance);
    _posSmooth = Target.position;

    Vector3 newPosition = rotation * new Vector3(0.0f, 0.0f, -Distance) + _posSmooth;

    // These clamps were specific to VENU. May not be needed at all... 
    // or might need to be rethought
    newPosition.x = Mathf.Clamp(newPosition.x, -10, 10);
    newPosition.z = Mathf.Clamp(newPosition.z, -50, 50);

    transform.position = newPosition;
	}

  private void Update()
  {
    if (Input.touchCount == 2 )
    { //Missing 2 conditionals for the event system
      //Store both touches
      Touch touchZero = Input.GetTouch(0);
      Touch touchOne = Input.GetTouch(1);

      // Find the position in the previous frame of each touch.
      Vector2 touchZeroLastPos = touchZero.position - touchZero.deltaPosition;
      Vector2 touchOneLastPos = touchOne.position - touchOne.deltaPosition;

      // Find the distance between the touches in each frame
      Vector2 lastTouchDistance = touchZeroLastPos - touchOneLastPos;
      float prevTouchDeltaMag = lastTouchDistance.magnitude;
      Vector2 currentTouchDistance = touchZero.position - touchOne.position;
      float currentTouchDeltaMag = currentTouchDistance.magnitude;

      float deltaMagDifference = prevTouchDeltaMag - currentTouchDeltaMag;

      Distance += deltaMagDifference * 0.1f;


      // Find th difference in distances between eac frame; 

    }
  }
}
