///<summary>
/// DrawModeControlManager.cs - This class contains all of the user functionality for the draw mode. 
/// 
/// Copyright - VARIAL Studios LLC
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
  private const int MAX_BRUSH_COUNT = 1000;                 //To avoid having millions of brushes
  private int _brushCounter = 0;
  private DrawMode _drawMode;                               // The current draw mode for the UI. 
  private Vector2 _touchStartPosition;                      // Touch start position 
  private Stack<Stroke> _strokeStack = new Stack<Stroke>(); // A stack of brush strokes from the user
  private Stack<Stroke> _redoStack = new Stack<Stroke>();   // A stack of strokes for holding on to the undone strokes 
  private Material _blankCanvas;                            // Saved canvas for starting a new drawing. 
  private int _drawColorEnum = 0;                           // 0 - black, 1 - white, 2 - red, 3 - green, 4 - blue 
  private Texture2D _tex;                                   // The Texture of the copy plane that you draw on.
  private bool _hasUndoneAction = false;                    // The boolean is for activating the undone button 
  private GameObject _copyPlane;                            // The copy object (so we don't overwright the original)
  private string _fileName;                                 // The name of the saved file
  
private void Start()
  {
    _drawMode = DrawMode.Idle;
    
    // Initialize the brush slider, and set the values 
    BrushSizeSlider.minValue = 2;
    BrushSizeSlider.maxValue = 12;
    BrushSizeSlider.value = BrushSize;

    // Hide the Undo and Redo Buttons initially
    UndoButton.gameObject.SetActive(false);
    RedoButton.gameObject.SetActive(false);


  }

  /// <summary>
  /// Public method to invoke the coroutine to save the texture to a datafile. 
  /// </summary>
  public void SaveTexture() {
    _brushCounter = 0;
    System.DateTime date = System.DateTime.Now;
    RenderTexture.active = canvasTexture;
    Texture2D tex = new Texture2D(canvasTexture.width, canvasTexture.height, TextureFormat.RGB24, false);
    tex.ReadPixels(new Rect(0, 0, canvasTexture.width, canvasTexture.height), 0, 0);
    tex.Apply();
    RenderTexture.active = null;
    _fileName = date.ToString();
    StartCoroutine ("SaveTextureToFile"); //SAVING
    // Remove the copy plane from the scene
    Destroy(_copyPlane);
    // rather than doing that I should create a method that clears and resets. 

    // INVOKE THE CNN ROUTINE

    // Control manager should call this. 

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
  /// Undo the last brush stroke
  /// </summary>
  public void UndoLastStroke()
  {
    Debug.Log("Undo Pressed!");
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
      _tex.SetPixel((int)point.x, (int)point.y, Color.black);
    _strokeStack.Push(redoneStroke);
    _brushCounter++;
    
    // If the redo stack is empty at the end of the run, deactivate the button
    if (_redoStack.Count == 0)
      RedoButton.gameObject.SetActive(false);
  }

  /// <summary>
  /// Saves the texture to the file. 
  /// </summary>
  /// <param name="savedTexture"></param>
  /// <returns></returns>
  IEnumerator SaveTextureToFile(Texture2D savedTexture)
  {
    _brushCounter = 0;
    string fullPath = System.IO.Directory.GetCurrentDirectory() + "\\UserCanvas\\";
    System.DateTime date = System.DateTime.Now;
    if (!System.IO.Directory.Exists(fullPath))
      System.IO.Directory.CreateDirectory(fullPath);
    var bytes = savedTexture.EncodeToPNG();
    System.IO.File.WriteAllBytes(fullPath + _fileName, bytes);
    Debug.Log("<color=orange>Saved Successfully!</color>" + fullPath + _fileName);

    // Return to the edit mode, and wait for the result. 

    yield return null;
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
      if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved)
      {
        _touchStartPosition = pixelUV;
      }

      for (int i = -BrushSize / 2; i <= BrushSize / 2; ++i)
      {
        for (int j = -BrushSize / 2; j <= BrushSize / 2; ++j)
        {
          //If the pixel exists, then we change it
          if (_tex.GetPixel((int)pixelUV.x + i, (int)pixelUV.y + j) != null && _touchStartPosition != null)
          {
            Vector2 drawPoint = new Vector2((int)pixelUV.x + i, (int)pixelUV.y + j);
            LineDrawer.DrawLine(_tex, drawPoint, _touchStartPosition, Color.black);
            LineDrawer.DrawLine(_tex, 2); // TEST
            newStrokeDrawPointList.Add(drawPoint);
          }
        }
      }
      if (Input.GetTouch(0).phase == TouchPhase.Ended)
      {
        newStroke.StrokeUpdateCoords = newStrokeDrawPointList;
        _strokeStack.Push(newStroke);
        _brushCounter++;
      }
      _tex.Apply();
      _touchStartPosition = pixelUV;
    }
  }
}
