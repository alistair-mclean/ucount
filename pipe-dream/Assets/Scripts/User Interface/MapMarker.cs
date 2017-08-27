///<summary>
/// MapMarker.cs - follows the user on the map
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MapMarker : MonoBehaviour {
  // Public
  public Transform TargetObject;
  // Private
  private Transform _marker;

  /// <summary>
  /// Set the gameobject.
  /// </summary>
  /// <returns></returns>
  private IEnumerator Start()
  {
    _marker = GetComponent<Transform>();
    yield return null;
  }

  /// <summary>
  /// Move the marker to the object position
  /// </summary>
  private void LateUpdate()
  {
    _marker.transform.position = TargetObject.position;
    //Ensure that the marker is above the map
    _marker.transform.position = new Vector3(_marker.transform.position.x,
                                             _marker.transform.position.y + 1f,
                                             _marker.transform.position.z);
  }


}
