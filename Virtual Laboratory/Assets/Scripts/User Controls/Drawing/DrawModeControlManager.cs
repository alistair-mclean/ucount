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
  public enum DrawMode { Draw, Idle };// The draw mode
  [Range(2, 10)]
  public int BrushSize = 2;           // width/height of the users brush. (It's a square for now)
  public RenderTexture canvasTexture; // Render Texture that looks at our Base Texture and the painted brushes
  public Slider BrushSizeSlider;      // The UI slider to control the brush size
  public Button DrawingDotButton;     // Color switching might not be a good idea.... 
  public Button UndoButton;           // Undo Button UI Element
  public Button RedoButton;           // Redo Button UI element 
  public Button FinishedButton;       // The finished button, the user presses this when they are done with their drawing
  public Material baseMaterial;       // The material of our base texture (Were we will save the painted texture)
  public Image DrawIconImage;         // The draw icon image for the draw button 
  public Material CanvasMaterial;     // The canvas that you draw on 
  public GameObject DrawingContainer; // Container for the instantiated canvas texture
  public GameObject DrawingPlane;     // Drawing plane prefab

  // Private 
  private int _brushCounter = 0, MAX_BRUSH_COUNT = 1000; //To avoid having millions of brushes
  private DrawMode _drawMode; // The current draw mode for the UI. 
  private Color _brushColor; // Black for draw, White for Erase
  private Vector2 _touchStartPosition; 
  private Stack<Stroke> _strokeStack = new Stack<Stroke>();
  private Stack<Stroke> _redoStack = new Stack<Stroke>(); // a stack of strokes for holding on to when a user undoes an action, that way we can add it back with redo
  private Material _blankCanvas; // Saved canvas for starting a new drawing. 
  private int _drawColorEnum = 0; // 0 - black, 1 - white, 2 - red, 3 - green, 4 - blue 
  private Texture2D _tex;
  private bool _hasUndoneAction = false;
  private GameObject _copyPlane;
  
private void Start()
  {
    _drawMode = DrawMode.Idle;
    _brushColor = Color.black;
    
    // Initialize the brush slider, and set the values 
    BrushSizeSlider.minValue = 2;
    BrushSizeSlider.maxValue = 12;
    BrushSizeSlider.value = BrushSize;
  }

  /// <summary>
  /// Save the texture to a datafile. 
  /// </summary>
  void SaveTexture() {
    _brushCounter = 0;
    System.DateTime date = System.DateTime.Now;
    RenderTexture.active = canvasTexture;
    Texture2D tex = new Texture2D(canvasTexture.width, canvasTexture.height, TextureFormat.RGB24, false);
    tex.ReadPixels(new Rect(0, 0, canvasTexture.width, canvasTexture.height), 0, 0);
    tex.Apply();
    RenderTexture.active = null;
    baseMaterial.mainTexture = tex; //Put the painted texture as the base

    //StartCoroutine ("SaveTextureToFile"); //SAVING
    Invoke("ShowCursor", 0.1f);
  }

  /// <summary>
  /// Sets whether the draw mode is on or off. 
  /// </summary>
  /// <param name="isOn"> true = On, false = Off</param>
  public void SetDrawMode(bool isOn)
  {
   if(!isOn)
        _drawMode = DrawMode.Idle;
        
   else { 
        _drawMode = DrawMode.Draw;
      _copyPlane = GameObject.Instantiate(DrawingPlane);
    }
  }

  /// <summary>
  /// Iterates to the next color as the user presses on the draw icon button.
  /// </summary>
  public void NextBrushColor()
  {
    _drawColorEnum++;
    if (_drawColorEnum > 4)
      _drawColorEnum = 0;
    switch (_drawColorEnum)
    {
      case (0):
        _brushColor = Color.black;
        break;
      case (1):
        _brushColor = Color.white;
        break;
      case (2):
        _brushColor = Color.red;
        break;
      case (3):
        _brushColor = Color.green;
        break;
      case (4):
        _brushColor = Color.blue;
        break;
    }
    DrawIconImage.color = _brushColor;
  }
  
  /// <summary>
  /// Undo the last brush stroke
  /// </summary>
  public void UndoLastStroke()
  {
    // Leave if the stack is empty
    if (_strokeStack.Count == 0)
      return;
    int size = _strokeStack.Count;

    //Pop off the last stroke and put it in the _redoStack
    Stroke undoneStroke = _strokeStack.Pop();
    _redoStack.Push(undoneStroke); 

    //Set the pixels back
    foreach (Vector2 point in undoneStroke.StrokeUpdateCoords)
      _tex.SetPixel((int)point.x, (int)point.y, Color.white);
    _brushCounter--;

    //Activate the redo button
    if (!RedoButton.IsActive()) 
      RedoButton.gameObject.SetActive(true);

    //If the brushcounter is 0 at the end of the run, deactivate the button - PERHAPS WE SHOULD DO THIS ELSEWHERE?? 
    if (_brushCounter == 0)
      UndoButton.gameObject.SetActive(false);
  }

  /// <summary>
  /// Redo the last brush stroke. If there are no 
  /// </summary>
  public void RedoLastStroke()
  {
    if (_redoStack.Count == 0)
      return;
    Stroke redoneStroke = _redoStack.Pop();
    //Put the pixels back to their color. 
    foreach (Vector2 point in redoneStroke.StrokeUpdateCoords)
      _tex.SetPixel((int)point.x, (int)point.y, redoneStroke.BrushColor);
    _strokeStack.Push(redoneStroke);
    _brushCounter++;
    
    // If the redo stack is empty at the end of the run, deactivate the button
    if (_redoStack.Count == 0)
      RedoButton.gameObject.SetActive(false);
  }

  /// <summary>
  /// UPDATE
  /// If the user touches on the canvas, draw a line over where their finger touches. 
  /// </summary>
  void Update()
  {
    // Leave if the user isn't touching, or if the draw mode is in idle  
    if (!Input.GetMouseButton(0))
      return;
    if (_drawMode == DrawMode.Idle)
      return;

    BrushSize = (int)BrushSizeSlider.value;

    // Turn on the undo button if there are brush counts 
    if (_brushCounter > 0 && !UndoButton.gameObject.activeInHierarchy)
      UndoButton.gameObject.SetActive(true);


    if (Input.touchCount > 0)
    {
      RaycastHit hit;
      if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        return;
      Renderer rend = hit.transform.GetComponent<Renderer>();
      MeshCollider meshCollider = hit.collider as MeshCollider;

      if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
        return;

      Texture2D hitTexture = rend.material.mainTexture as Texture2D;
      _tex = hitTexture;
      Vector2 pixelUV = hit.textureCoord;
      pixelUV.x *= _tex.width;
      pixelUV.y *= _tex.height;
      Color[] textureColorArray = _tex.GetPixels();
      Stroke newStroke = new Stroke();
      List<Vector2> newStrokeDrawPointList = new List<Vector2>();
      newStroke.StrokeID = _brushCounter;
      newStroke.BrushStrokeSize = BrushSize;
      newStroke.BrushColor = _brushColor;
      if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved)
      {
        _touchStartPosition = pixelUV;
      }

      for (int i = -BrushSize / 2; i <= BrushSize / 2; ++i)
      {
        for (int j = -BrushSize / 2; j <= BrushSize / 2; ++j)
        {
          if (_tex.GetPixel((int)pixelUV.x + i, (int)pixelUV.y + j) != null && _touchStartPosition != null)
          {
            //if the pixel exists, then we change it
            // tex.SetPixel((int)pixelUV.x + i, (int)pixelUV.y + j, _drawColor);
            Vector2 drawPoint = new Vector2((int)pixelUV.x + i, (int)pixelUV.y + j);
            LineDrawer.DrawLine(_tex, drawPoint, _touchStartPosition, _brushColor);
            newStrokeDrawPointList.Add(drawPoint);
          }
        }
      }
      Debug.Log("right before touchphase.ended");
      if (Input.GetTouch(0).phase == TouchPhase.Ended)
      {
        newStroke.StrokeUpdateCoords = newStrokeDrawPointList;
        _strokeStack.Push(newStroke);
        _brushCounter++;
        Debug.Log("Stroke count = " + _strokeStack.Count);
      }
      Debug.Log("right after touchphase.ended");
      _tex.Apply();
      _touchStartPosition = pixelUV;
    }
  }
}
