using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CorrectPerspectiveHeight : MonoBehaviour {
  // Public
  public WrldMap WRLDMAP;
  public static float HEIGHT = 0.1f;

  // Private
  private Vector3 m_position;
  private Vector2 m_gpsCoordinates;
  private Ray m_ray; //This ray looks downward.

  private void Start()
  {
    m_position = transform.position;
    m_ray.origin = m_position;
    m_ray.direction = Vector3.down;
  }

  void Update () {
    RaycastHit hit;

    //Ensure that the user is above the map, at the specified height.
    if (Physics.Raycast(m_ray, out hit))
    {
      print("The map is below the user.");

      if (hit.distance > HEIGHT)
      {
        // WE ARE TOO FAR ABOVE THE MAP!
        // Look for the map, it's terrain, and locate the proper position to assign the user to depending upon their GPS location.
      }
    }

  }

  Vector3 LocatePositionOnMap(Vector2 currentLocation, WrldMap currentMap)
  {
    // This is the method we will use to evaluate where we should place the user on the WRLD map, and therefore the Unity world space.
    Vector3 positionOnWrldMap = new Vector3(0, 0, 0);



    return positionOnWrldMap;
  }


}
