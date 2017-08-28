using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSection  {

  // Private
  public float Diameter { get; set; }
  public float Length { get; set; }
  public Vector3 StartPostion { get; set; } //long, lat, and depth
  public Vector3 EndPostion { get; set; } // long, lat, and depth

  public PipeSection(float diam, float length, Vector3 startPos, Vector3 endPos)
  {
    // Validation
    if (diam <= 0.0f)
      Debug.LogError("Error: Diameter too small! (d <= 0)");
    if (length <= 0.0f || length <= diam)
      Debug.LogError("Error: Length too small! (l <= 0 || l <= d)");
    if (startPos == endPos)
      Debug.LogError("Error: Pipe section starting position is the same as the end position");

    // Initialization
    Diameter = diam;
    Length = length;
    StartPostion = startPos;
    EndPostion = endPos;
  }


}
