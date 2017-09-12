using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UserLocationOnMap : MonoBehaviour
{
  public Text LocationTextbox;
  public Text AltitudeTextbox;
  public Text AccuracyTextbox;
  public Text StatusTextbox;

  private static float _lat;
  private static float _long;
  private static float _altitude;
  private static float _horizontalAccuracy;
  private static float _verticalAccuracy;
  private static LocationService _locService;

  private void Start()
  {
    Input.location.Start();
  }

  private void Update() {
    Debug.Log("status of input location service:" + Input.location.status);
    _lat = Input.location.lastData.latitude;
    _long = Input.location.lastData.longitude;
    _horizontalAccuracy = Input.location.lastData.horizontalAccuracy;
    _verticalAccuracy = Input.location.lastData.verticalAccuracy;
    StartCoroutine(SetText());
  }

  private IEnumerator SetText()
  {
    Debug.Log("set Text called!");
    LocationTextbox.text = "( " + _lat + ", " + _long + " )";
    AltitudeTextbox.text = _altitude + "m";
    AccuracyTextbox.text = _horizontalAccuracy + " , " + _verticalAccuracy;
    StatusTextbox.text = Input.location.status.ToString();
    yield return null;
  }
}
