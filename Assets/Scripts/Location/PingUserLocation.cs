using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PingUserLocation : MonoBehaviour {
  // Public
  public Text LocationText;


  // Private
  private bool _buttonPressed = false;

	void Start () {
    LocationText.text = "Location service offline";
	}

  // Method for the user button
  public void StartButtonActivate()
  {
    if (!_buttonPressed) {
      StartCoroutine(StartLocation());
      _buttonPressed = true;
    }
  }

  public void StopButtonActivate()
  {
    if (_buttonPressed && Input.location.status == LocationServiceStatus.Running)
    {
      StartCoroutine(StopLocation());
      _buttonPressed = false;
    }
  }


  IEnumerator StartLocation()
  {
    // First, check if user has location service enabled
    if (!Input.location.isEnabledByUser)
      yield break;

    // Start service before querying location
    Input.location.Start();

    // Wait until service initializes
    int maxWait = 20;
    while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
    {
      yield return new WaitForSeconds(1);
      maxWait--;
    }

    // Service didn't initialize in 20 seconds
    if (maxWait < 1)
    {
      print("Timed out"); // Warning
      yield break;
    }

    // Connection has failed
    if (Input.location.status == LocationServiceStatus.Failed)
    {
      print("Unable to determine device location"); // Warning
      yield break;
    }
    else
    {
      // Access granted and location value could be retrieved
      LocationText.text = "Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp; // DEBUG

      // Correct the AR perspective height
      if (gameObject.GetComponent<CorrectPerspectiveHeight>()) {
        gameObject.GetComponent<CorrectPerspectiveHeight>().Relocate(Input.location.lastData.latitude,
                                                                      Input.location.lastData.longitude,
                                                                      Input.location.lastData.altitude);
      }
    }

  }


  // Stop the service
  IEnumerator StopLocation()
  {
    Input.location.Stop();
    yield return null;
  }
}
