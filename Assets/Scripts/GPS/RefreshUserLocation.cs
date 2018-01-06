using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// RefreshUserLocation.cs
/// - Refreshes the user location with the wrld map at a fixed rate.
/// - Location based functions are all run as coroutines, for now.
/// - Stops the location service when the application is paused.
/// - Resumes the location service when
/// </summary>

public class RefreshUserLocation : MonoBehaviour {
  public static int UpdateFrequency = 10; // **CHANGE THIS VALUE OTHERWISE WE WILL GET BANNED FROM GOOGLES SERVERS**
  //NEED TO ADD A REQUIRE HERE
  public WrldMap WORLDMAP;

  private LocationService _locationService; // MAY BE REDUNDANT
  // Use this for initialization

  IEnumerator Start()
  {
    _locationService = new LocationService();
    // First, check if user has location service enabled
    if (!Input.location.isEnabledByUser)
      yield break;

    // Start service before querying location
    Input.location.Start();
    _locationService.Start(); // MAY BE REDUNDANT

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
      print("Timed out");
      yield break;
    }

    // Connection has failed
    if (Input.location.status == LocationServiceStatus.Failed)
    {
      print("Unable to determine device location");
      yield break;
    }
    else
    {
      // Access granted and location value could be retrieved
      print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
    }

  }

  private IEnumerator OnApplicationPause(bool pause)
  {
    // Stop location service
    // -: To avoid unneccessary queries to location updates on pause.
    Input.location.Stop();
    _locationService.Stop(); // MAY BE REDUNDANT
    yield return null;
  }

  private IEnumerator OnApplicationFocus(bool focus)
  {
    // Resume location service
    // -: On application focus, resume activity as normal.
    Input.location.Start();
    _locationService.Start(); // MAY BE REDUNDANT
    yield return null;
  }



  //TODO - figure out what to put here
  private void FixedUpdate()
  {

  }



}
