/// <summary>
/// EditModeControlManager.cs - Container for all user controls, enabling 
/// and disabling components when neccessary. 
/// 
/// Copyright - VARIAL Studios LLC
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum EditMode { Idle, Draw, Camera };

public class EditModeControlManager : MonoBehaviour {
  // Public
  public GameObject DrawingPlane; // The object plane the user draws on.
  public GameObject DefaultModePanel; // The UI panel containing the default mode controls 
  public GameObject CameraFeedPanel; // The object plane the camera feed is displayed on. 
  public GameObject DrawModePanel; // The UI panel containing the draw mode controls
  public GameObject CameraModePanel; // The UI panel containing the camera mode controls/display
  public Button DrawButton;
  public Button EraseButton;

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
      //DEFAULT MODE
      case (0):
        if (_userEditMode == EditMode.Idle)
          return;
        _userEditMode = EditMode.Idle;
        if (!DrawingPlane.activeInHierarchy)
          DrawingPlane.SetActive(true);
        
        DrawModePanel.SetActive(false);
        CameraFeedPanel.SetActive(false);

        GetComponent<DrawModeControlManager>().SetDrawMode(false);
        GetComponent<PhoneCamera>().CameraIsOn = false;
        break;

      //DRAW MODE
      case (1):
        if (_userEditMode == EditMode.Draw)
          return;
        _userEditMode = EditMode.Draw;

        if (!DrawingPlane.activeInHierarchy) //may be redundant
          DrawingPlane.SetActive(true);
        DrawModePanel.SetActive(true);
        DefaultModePanel.SetActive(false);
        CameraFeedPanel.SetActive(false);

        GetComponent<DrawModeControlManager>().SetDrawMode(true);
      break;

      //CAMERA MODE
      case (2):
        if (_userEditMode == EditMode.Camera)
          return;
        _userEditMode = EditMode.Camera;
        
        CameraFeedPanel.SetActive(true);
        DefaultModePanel.SetActive(false);  
        DrawingPlane.SetActive(false);

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
