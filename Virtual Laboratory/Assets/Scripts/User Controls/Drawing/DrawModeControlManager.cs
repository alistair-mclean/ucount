///<summary>
/// DrawModeControlManager.cs - This class contains all of the user functionality for the draw mode. 
///</summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class DrawModeControlManager : MonoBehaviour {
  // Public
  public enum DrawMode { Draw, Erase, Idle };

  [Range(2, 10)]
  public int BrushSize = 2;
  public RenderTexture canvasTexture; // Render Texture that looks at our Base Texture and the painted brushes
  public Material baseMaterial;       // The material of our base texture (Were we will save the painted texture)
  public Material CanvasMaterial;     // The canvas that you draw on 
  public Image DrawIconImage;         // The draw icon image for the draw button 

  // Private 
  private int _brushCounter = 0, MAX_BRUSH_COUNT = 1000; //To avoid having millions of brushes
  private DrawMode _drawMode;
  private Color _drawColor; // Black for draw, White for Erase
  private Vector2 _touchStartPosition; 
  private Stack<Stroke> _strokeStack;
  private Material _blankCanvas; // Saved canvas for starting a new drawing. 
  private int _drawColorEnum = 0; // 0 - black, 1 - white, 2 - red, 3 - green, 4 - blue 

  private void Start()
  {
    _drawMode = DrawMode.Idle;
    _drawColor = Color.white;
  }

  void Update()
  {
    if (!Input.GetMouseButton(0))
      return;
    if (_drawMode == DrawMode.Idle)
      return;
    if (Input.touchCount > 0)
    {
      RaycastHit hit;
      if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        return;
      Renderer rend = hit.transform.GetComponent<Renderer>();
      MeshCollider meshCollider = hit.collider as MeshCollider;

      if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
        return;

      Texture2D tex = rend.material.mainTexture as Texture2D;
      Vector2 pixelUV = hit.textureCoord;
      pixelUV.x *= tex.width;
      pixelUV.y *= tex.height;
      Color[] textureColorArray = tex.GetPixels();
      Stroke newStroke;
      newStroke.StrokeID = _brushCounter;

      if (Input.GetTouch(0).phase == TouchPhase.Began)
      {
        _touchStartPosition = pixelUV;

      }

      if (Input.GetTouch(0).phase == TouchPhase.Moved)
      {

      }

      if (Input.GetTouch(0).phase == TouchPhase.Ended)
      {

        _brushCounter++;
      }

      for (int i = -BrushSize / 2; i <= BrushSize / 2; ++i)
      {
        for (int j = -BrushSize / 2; j <= BrushSize / 2; ++j)
        {
          if (tex.GetPixel((int)pixelUV.x + i, (int)pixelUV.y + j) != null && _touchStartPosition != null)
          {
            //if the pixel exists, then we change it
            // tex.SetPixel((int)pixelUV.x + i, (int)pixelUV.y + j, _drawColor);
            Vector2 drawPoint = new Vector2((int)pixelUV.x + i, (int)pixelUV.y + j);
            LineDrawer.DrawLine(tex, drawPoint, _touchStartPosition, Color.black);
          }
        }
      }
      tex.Apply();
    }



    //RaycastHit hit;
    //if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
    //  return;
    //Renderer rend = hit.transform.GetComponent<Renderer>();
    //MeshCollider meshCollider = hit.collider as MeshCollider;

    //if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
    //  return;

    //Texture2D tex = rend.material.mainTexture as Texture2D;
    //Vector2 pixelUV = hit.textureCoord;
    //pixelUV.x *= tex.width;
    //pixelUV.y *= tex.height;
    //Color[] textureColorArray = tex.GetPixels();


  }
  

  //Sets the base material with a our canvas texture, then removes all our brushes
  void SaveTexture()
  {
    _brushCounter = 0;
    System.DateTime date = System.DateTime.Now;
    RenderTexture.active = canvasTexture;
    Texture2D tex = new Texture2D(canvasTexture.width, canvasTexture.height, TextureFormat.RGB24, false);
    tex.ReadPixels(new Rect(0, 0, canvasTexture.width, canvasTexture.height), 0, 0);
    tex.Apply();
    RenderTexture.active = null;
    baseMaterial.mainTexture = tex; //Put the painted texture as the base

    //StartCoroutine ("SaveTextureToFile"); //Do you want to save the texture? This is your method!
    Invoke("ShowCursor", 0.1f);
  }

  public void SetDrawMode(int newMode)
  {
    if (newMode < 0 || newMode > 2)
    {
      Debug.LogError("Error in DrawOnFingerTouch.SetDrawMode: New Mode out of range! Set values between 0 and 2.");
    }

    switch (newMode)
    {
      case (0):
        _drawMode = DrawMode.Idle;
        break;
      case (1):
        _drawMode = DrawMode.Draw;
        _drawColor = Color.black;
        break;
      case (2):
        _drawMode = DrawMode.Erase;
        _drawColor = Color.white;
        break;
    }
  }

  /// <summary>
  /// Iterates to the next color as the user presses on the draw icon button
  /// </summary>
  public void NextDrawColor()
  {
    _drawColorEnum++;
    if (_drawColorEnum > 4)
      _drawColorEnum = 0;
    switch (_drawColorEnum)
    {
      case (0):
        _drawColor = Color.black;
        break;
      case (1):
        _drawColor = Color.white;
        break;
      case (2):
        _drawColor = Color.red;
        break;
      case (3):
        _drawColor = Color.green;
        break;
      case (4):
        _drawColor = Color.blue;
        break;
    }
    DrawIconImage.color = _drawColor;
  }
}
