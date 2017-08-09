using UnityEngine;
using System.Collections;

// Test summary - This class draws a line when the user presses space. The line originates
// at the mouse position when space is pressed and extends out to the current mouse position.
// Conclusion - I don't think this is what I need to solve this issue. 

public class GLLineTest : MonoBehaviour
{
  public Material mat;
  private Vector3 startVertex;
  private Vector3 mousePos;
  void Update()
  {
    mousePos = Input.mousePosition;
    if (Input.GetKeyDown(KeyCode.Space))
      startVertex = new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height, 0);

  }
  void OnPostRender()
  {
    if (!mat)
    {
      Debug.LogError("Please Assign a material on the inspector");
      return;
    }
    GL.PushMatrix();
    mat.SetPass(0);
    GL.LoadOrtho();
    GL.Begin(GL.LINES);
    GL.Color(Color.red);
    GL.Vertex(startVertex);
    GL.Vertex(new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height, 0));
    GL.End();
    GL.PopMatrix();
  }
  void Example()
  {
    startVertex = new Vector3(0, 0, 0);
  }
}