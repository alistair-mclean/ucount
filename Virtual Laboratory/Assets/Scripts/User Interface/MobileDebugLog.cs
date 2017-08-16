///<summary>
/// MobileDebugLog.cs - Acts as a console for mobile devices only, disabled in editor mode and standalone testing.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MobileDebugLog : MonoBehaviour {
  // Public
  public Text LogTextBox;

  // Private
  private Stack<string> _log;

  private void Start()
  {
    if (!LogTextBox)
      return;
    LogTextBox.text = "Mobile Debug Log: ";
  }

  private void Update()
  {
   string logMessage = Debug.unityLogger.ToString();

    _log.Push(logMessage);

  }

}
