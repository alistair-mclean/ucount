///<summary>
/// UserMapMarker.cs - Keeps the map marker icon on the user location.
///</summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMapMarker : MonoBehaviour {
  // Public
  public Transform UserLoc;

	void LateUpdate () {
    transform.position = UserLoc.position;
	}
}
