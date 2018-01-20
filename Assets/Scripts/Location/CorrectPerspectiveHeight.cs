using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wrld;
using Wrld.Space;

/// <summary>
/// CorrectPerspectiveHeight.cs
/// Corrects the position of the perspective with respect to the WRLD map.
///
/// SHOULD PROBABLY BE RENAMED AT SOME POINT
/// </summary>


public class CorrectPerspectiveHeight : MonoBehaviour {
  // Public
  public float HEIGHT = 10.0f;
  public float DELAYTIME = 2.0f;
  public Transform MapCameraPosition;
  public GeographicTransform coordinateFrame;

  // Private
  private static LatLongAltitude _latLongAlt = LatLongAltitude.FromDegrees(40.025147, -105.285932, 1646); //Hardcoded initial value
  private static LatLong _latLong = LatLong.FromDegrees(40.025147, -105.285932); //Hardcoded initial value
  private Ray _ray;

  private void Start()
  {
    Api.Instance.GeographicApi.RegisterGeographicTransform(coordinateFrame);
  }


  private IEnumerator MoveObjectToLatLongAlt(LatLong newLatLong, float alt)
  {
    MapCameraPosition.position = new Vector3(0.0f, HEIGHT, 0.0f);

    while(true)
    {
      yield return new WaitForSeconds(DELAYTIME);
      coordinateFrame.SetPosition(newLatLong);
      transform.position = new Vector3(transform.position.x, alt, transform.position.y); // Hack to fix the altitude
    }

  }

  //This method is used when a button is pressed to send the user's location.
  public void Relocate(double newLat, double newLong, float newAlt)
  {
    _latLong = LatLong.FromDegrees(newLat, newLong);
    StartCoroutine(MoveObjectToLatLongAlt(_latLong, newAlt));
  }
}
