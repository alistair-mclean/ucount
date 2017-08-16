using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CameraModeManager : MonoBehaviour
{
  // Public
  public bool CameraAvailable;          // Is the camera available?
  public AspectRatioFitter fit;         // Aspect ratio fitter to keep ratio consistent with device
  public RawImage Background;           // The background image that gets rendered on
  public bool CameraIsOn = false;       // Is the camera on?
  public Image SaveSuccessfullImage;    // This image gets activated whenever the save is succesful.

  // Private
  private string _fileName;             // The name of the save file
  private WebCamTexture _backCam;       // The webcam texture
  private Texture _defaultBackground;   // The default display background (if no camera is available)
  private Renderer _captureRenderer;    // The renderer needed to capture the image
  private Texture2D _captureTexture;


  /// <summary>
  /// Start
  /// -- Check to see if there are available devices.
  /// -- Check if the available devices are back-facing.
  /// --
  /// </summary>
  private void Start()
  {
    _captureRenderer = GetComponent<Renderer>();
    _defaultBackground = Background.texture;
    WebCamDevice[] devices = WebCamTexture.devices;

    if (devices.Length == 0)
    {
      Debug.Log("No camera detected.");
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


  public void CaputureImage()
  {
    _backCam.Pause();
    Texture placeholder = _backCam;
    _captureTexture = (Texture2D)placeholder;
    SaveTexture();
  }


  /// <summary>
  /// Public method to invoke the coroutine to save the texture to a datafile.
  /// </summary>
  public void SaveTexture()
  {
    System.DateTime date = System.DateTime.Now;
    //RenderTexture.active = canvasTexture;
    Texture2D tex = new Texture2D(_backCam.width, _backCam.height, TextureFormat.RGB24, false);
    tex.ReadPixels(new Rect(0, 0, _backCam.width, _backCam.height), 0, 0);
    tex.Apply();
    //RenderTexture.active = null;
    _fileName = date.ToString();
    StartCoroutine("SaveTextureToFile"); //SAVING
    // Remove the copy plane from the scene
    // rather than doing that I should create a method that clears and resets.

    // INVOKE THE CNN ROUTINE

    // Control manager should call this.

  }

  /// <summary>
  /// Saves the texture to the file.
  /// </summary>
  /// <param name="savedTexture"></param>
  /// <returns></returns>
  IEnumerator SaveTextureToFile()
  {
    string fullPath = System.IO.Directory.GetCurrentDirectory() + "\\UserCanvas\\";
    //System.DateTime date = System.DateTime.Now;
    //_fileName = date.ToShortDateString();
    _fileName = "newFile.png";
    if (!System.IO.Directory.Exists(fullPath))
      System.IO.Directory.CreateDirectory(fullPath);
    var bytes = _captureTexture.EncodeToPNG();
    System.IO.File.WriteAllBytes(fullPath + _fileName, bytes);
    Debug.Log("<color=orange>Saved Successfully!</color>" + fullPath + _fileName);

    // Return to the edit mode, and wait for the result.
    // MAKE A CALL TO THE EDITCONTROLMODEMANAGER
    _backCam.Play();
    SaveSuccessfullImage.gameObject.SetActive(true);
    //    GetComponent<EditModeControlManager>().SetUserEditMode(0);
    yield return null;
  }

  private void Update()
  {
    // Return back to default if escape is pressed
    if (Input.GetKey(KeyCode.Escape))
      GetComponent<EditModeControlManager>().SetUserEditMode(0);

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


