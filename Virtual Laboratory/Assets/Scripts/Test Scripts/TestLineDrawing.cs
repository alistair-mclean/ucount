using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLineDrawing : MonoBehaviour {

  public Vector3 startPos;
  public Vector3 endPos;

	
	// Update is called once per frame
	void Update () {
		
	}
  private void Start()
  {
    Debug.DrawLine(startPos, endPos);
  }
}
