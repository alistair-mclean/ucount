///<summary>
/// PhoneCamera.cs - 
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour {
  public bool CameraAvailable;
  public AspectRatioFitter fit;
  public RawImage Background;
  public bool CameraIsOn = false;


  private WebCamTexture _backCam;
  private Texture _defaultBackground;

  private void Start()
  {
    _defaultBackground = Background.texture;
    WebCamDevice[] devices = WebCamTexture.devices;

    if (devices.Length == 0)
    {
      Debug.Log("No camera detectd.");
      CameraAvailable = false;
      return;
    }

    for (int i = 0; i < devices.Length; i++)
    {
      if (!devices[i].isFrontFacing)
      {
        _backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
      }
    }
    if (_backCam == null)
    {
      Debug.Log("Unable to find back camera.");
      return;
    }
    _backCam.Play();
    Background.texture = _backCam;
    CameraAvailable = true;
  }

  private void Update()
  {
    if (!CameraAvailable || !CameraIsOn)
      return;
    float aspectRatio = (float)_backCam.width / (float)_backCam.height;
    fit.aspectRatio = aspectRatio;

    float scaleY = _backCam.videoVerticallyMirrored ? -1f : 1f;
    Background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

    int orient = -_backCam.videoRotationAngle;
    Background.rectTransform.localEulerAngles = new Vector3(0f, 0f, orient);
  }
}
