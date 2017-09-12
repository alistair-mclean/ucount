using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroCamera : MonoBehaviour {
  // Public
  public GameObject CameraContainer;

  // Private
  private bool _gyroEnabled;
  private Gyroscope _gyro;
  private Quaternion _rotation;

  void Start () {
    _gyroEnabled = EnableGyro();
	}

  private bool EnableGyro()
  {
    if (SystemInfo.supportsGyroscope)
    {
      _gyro = Input.gyro;
      _gyro.enabled = true;
      CameraContainer.transform.rotation = Quaternion.Euler(90f, 90f, 0);
      _rotation = new Quaternion(0, 0, 1, 0);
      return true;
    }
    return false;
  }

	void Update () {
    if (_gyroEnabled)
    {
      transform.localRotation = _gyro.attitude * _rotation;
    }
	}
}
