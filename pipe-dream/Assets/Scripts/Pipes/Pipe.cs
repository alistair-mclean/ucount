///<summary>
/// Pipe.cs - The pipe object class.
///</summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour {
  // Public
  public enum TransportMaterial { ClearWater, BlackWater, GrayWater, Electrical, Oil, Hazhardous}; // We can revisit these classifications later once we have more data
  public GameObject PipeObject; // The visual representation of the pipe

  // Private
  private string _owner { get; set; }
  private string _pipeMaterial { get; set; }
  private TransportMaterial _transportMaterial { get; set; }
  private Color _pipeDrawColor; //dependent upon material
  private float _diameter { get; set; }
  private float _length { get; set; }
  private List<PipeSection> _pipeLine = new List<PipeSection>();


  public Pipe(string owner, string mat, List<PipeSection> pipeSections)
  {
    // Validation
    if (owner.Length == 0)
      Debug.LogError("Error: No Pipe owner given.");
    if (mat.Length == 0)
      Debug.LogError("Error: No Pipe material given.");
    if (pipeSections.Count == 0)
      Debug.LogError("Error: Pipe segment list size is 0.");

    // Initialization
    _owner = owner;
    _pipeMaterial = mat;

    _pipeLine = pipeSections;
  }

 //Render the pipe
  public void DisplayPipe()
  {
    LineRenderer newLine = new LineRenderer();
    foreach (PipeSection section in _pipeLine)
    {
      Vector3[] positions = { section.StartPostion, section.EndPostion };
      newLine.SetPositions(positions);
      newLine.startWidth = section.Diameter;
      newLine.endWidth = section.Diameter;
      newLine.startColor = Color.red;
      newLine.endColor = Color.red;
      Instantiate(newLine);
    }
  }

}
