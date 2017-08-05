/// <summary>
///  Stroke.cs - Data structure for a brush stroke in the drawing mode. 
/// </summary>
using System.Collections.Generic;
using UnityEngine;

public struct Stroke  {
  public int StrokeID;
  public int BrushStrokeSize;
  public Color BrushColor;
  public List<Vector2> StrokeUpdateCoords;
}
