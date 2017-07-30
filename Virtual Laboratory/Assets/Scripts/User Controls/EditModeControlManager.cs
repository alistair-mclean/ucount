/// <summary>
/// EditModeControlManager.cs - Container for all user controls, enabling 
/// and disabling components when neccessary. 
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EditMode { Idle, Draw, Camera };

public class EditModeControlManager : MonoBehaviour {
  // Public
  public GameObject DrawingPlane;

  public GameObject CameraFeedPanel;
  public GameObject ButtonsPanel;
  public GameObject DrawButton;
  public GameObject EraseButton;

  private EditMode _userEditMode;

  private void Start()
  {
    SetUserEditMode(0);
  }

  /// <summary>
  /// SetUserEditMode - Assigns a new enumerated edit mode (if not already in it). 
  /// </summary>
  /// <param name="newMode"> </param>
  public void SetUserEditMode(int newMode)
  {
    if (newMode < 0 || newMode > 2)
    {
      Debug.LogError("EditModeControlManager Error in SetUserEditMode. New mode value is out of range! Set values at or between 0 and 2.");
    }
    switch (newMode)
    {
      case (0):
        if (_userEditMode == EditMode.Idle)
          return;
        _userEditMode = EditMode.Idle;
        if (!DrawingPlane.activeInHierarchy)
          DrawingPlane.SetActive(true);
        EraseButton.SetActive(false);
        CameraFeedPanel.SetActive(false);
        GetComponent<DrawOnFingerTouch>().SetDrawMode(0);
        GetComponent<PhoneCamera>().CameraIsOn = false;
        break;

      case (1):
        if (_userEditMode == EditMode.Draw)
          return;
        _userEditMode = EditMode.Draw;
        if (!DrawingPlane.activeInHierarchy) //may be redundant
          DrawingPlane.SetActive(true);

        CameraFeedPanel.SetActive(false);
        EraseButton.SetActive(true);

        GetComponent<DrawOnFingerTouch>().SetDrawMode(1);
      break;

      case (2):
        if (_userEditMode == EditMode.Camera)
          return;
        _userEditMode = EditMode.Camera;

        ButtonsPanel.SetActive(false);
        CameraFeedPanel.SetActive(true);
        DrawingPlane.SetActive(false);

        GetComponent<DrawOnFingerTouch>().SetDrawMode(0);
        GetComponent<PhoneCamera>().CameraIsOn = true;
        break;
    } 
  }
  private void Update()
  {
    if(Input.GetKeyDown(KeyCode.Escape))
    {
      if (_userEditMode == EditMode.Idle)
        SceneManager.LoadScene(0); //Go back to the main menu if already in idle
      SetUserEditMode(0); // Go back to idle if in any other mode
    }
  }



}
