using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR.WSA.WebCam;

public class CameraModeManager : MonoBehaviour {
  // Public
  public bool CameraAvailable;
  public AspectRatioFitter fit;
  public RawImage Background;
  public bool CameraIsOn = false;

  // Private
  private string _filename;



  private WebCamTexture _backCam;
  private Texture _defaultBackground;

  private void Start()
  {
    Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
    targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

    // Create a PhotoCapture object
    PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject) {
      photoCaptureObject = captureObject;
      CameraParameters cameraParameters = new CameraParameters();
      cameraParameters.hologramOpacity = 0.0f;
      cameraParameters.cameraResolutionWidth = cameraResolution.width;
      cameraParameters.cameraResolutionHeight = cameraResolution.height;
      cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

      // Activate the camera
      photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result) {
        // Take a picture
        photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
      });
    });

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
  PhotoCapture photoCaptureObject = null;
  Texture2D targetTexture = null;


  void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
  {
    // Copy the raw image data into the target texture
    photoCaptureFrame.UploadImageDataToTexture(targetTexture);

    // Create a GameObject to which the texture can be applied
    GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
    Renderer quadRenderer = quad.GetComponent<Renderer>() as Renderer;
    quadRenderer.material = new Material(Shader.Find("Custom/Unlit/UnlitTexture"));

    quad.transform.parent = this.transform;
    quad.transform.localPosition = new Vector3(0.0f, 0.0f, 3.0f);

    quadRenderer.material.SetTexture("_MainTex", targetTexture);

    // Deactivate the camera
    photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
  }

  void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
  {
    // Shutdown the photo capture resource
    photoCaptureObject.Dispose();
    photoCaptureObject = null;
  }
}
