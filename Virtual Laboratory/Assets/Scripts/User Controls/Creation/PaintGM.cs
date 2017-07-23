///<summary>
/// PaintGM.cs - General manager for painting in Unity
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintGM : MonoBehaviour {
  public enum ToolMode  { DrawMode, EraseMode, CameraMode, Idle };
  public GameObject TrailPrefab;
  public KeyCode MouseLeft;

  private GameObject _thisTrail;
  private Vector3 _startPos;
  private Plane _objectPlane;

  private ToolMode _mode;

	void Start () {
    _mode = ToolMode.Idle;
    _objectPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position);
	}
	
	void Update () {

    // Touch Controls
    if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButton(0))
    {
      _thisTrail = (GameObject)Instantiate(TrailPrefab,
                                           this.transform.position,
                                           Quaternion.identity);
      Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
      float rayDistance;
      if (_objectPlane.Raycast(mouseRay, out rayDistance)) {
        _startPos = mouseRay.GetPoint(rayDistance);
      }
    }  
    else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButtonDown(0))
    {
      Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
      float rayDistance;
      if (_objectPlane.Raycast(mouseRay, out rayDistance))
      {
        _thisTrail.transform.position = mouseRay.GetPoint(rayDistance);
      }
    }
    else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) || Input.GetMouseButtonUp(0))
    {
      if(Vector3.Distance(_thisTrail.transform.position, _startPos) < 0.1)
      {
        Destroy(_thisTrail);
      }
    }


  }
}
