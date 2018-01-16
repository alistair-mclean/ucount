using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wrld;
using Wrld.Space;

/// <summary>
/// RefreshUserLocation.cs
/// - Refreshes the user location with the wrld map at a fixed rate.
/// - Location based functions are all run as coroutines, for now.
/// - Stops the location service when the application is paused.
/// - Resumes the location service when
/// </summary>

public class RefreshUserLocation : MonoBehaviour {
  //Public
  public static int UpdateFrequency = 10; // **CHANGE THIS VALUE OTHERWISE WE WILL GET BANNED FROM GOOGLES SERVERS**
  public GeographicTransform CoordinateFrame;

  //Private
  private static float m_tempLat = 40.025164f;
  private static float m_tempLong = -105.285980f;
  private Transform m_userTransform;
  private LatLong m_tempLatLong;

  IEnumerator Start()
  {
    // Initializations
    m_userTransform = gameObject.transform;
    m_tempLatLong = LatLong.FromDegrees(m_tempLat, m_tempLong);

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


    var startLocation = LatLong.FromDegrees(Input.location.lastData.latitude, Input.location.lastData.longitude);

    Input.location.Stop();
    yield return null;
  }

  private void OnEnable()
  {
    Api.Instance.GeographicApi.RegisterGeographicTransform(CoordinateFrame);
    StartCoroutine(Example());
  }

  IEnumerator Example()
  {
    Api.Instance.CameraApi.MoveTo(m_tempLatLong, distanceFromInterest: 1000, headingDegrees: 0, tiltDegrees: 45);
    m_userTransform.localPosition = new Vector3(0.0f, 40.0f, 0.0f);

    while (true)
    {
      yield return new WaitForSeconds(2.0f);
      CoordinateFrame.SetPosition(m_tempLatLong);
    }
  }

  private void OnDisable()
  {
    StopAllCoroutines();
    Api.Instance.GeographicApi.UnregisterGeographicTransform(CoordinateFrame);
  }

  private IEnumerator OnApplicationPause(bool pause)
  {
    // Stop location service
    // -: To avoid unneccessary queries to location updates on pause.
    Input.location.Stop();
    yield return null;
  }

  private IEnumerator OnApplicationFocus(bool focus)
  {
    // Resume location service
    // -: On application focus, resume activity as normal.
    Input.location.Start();
    yield return null;
  }



  //TODO - figure out what to put here
  private void FixedUpdate()
  {

  }



}
