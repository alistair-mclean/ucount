﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOnFingerTouch : MonoBehaviour {
  // Public
  public int BrushSize = 2;
  public RenderTexture canvasTexture; // Render Texture that looks at our Base Texture and the painted brushes
  public Material baseMaterial; // The material of our base texture (Were we will save the painted texture)
  public Material canvasMaterial; // What you draw on - This is really fucking lazy... 
                                  // I just don't know how to properly do this in code at the moment 


  // Private 
  private int brushCounter = 0, MAX_BRUSH_COUNT = 1000; //To avoid having millions of brushes

  void Update()
  {
    if (!Input.GetMouseButton(0))
      return;

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
    Debug.Log("pixelUV: ( " + pixelUV.x + " , " + pixelUV.y + " )");
    Color[] textureColorArray = tex.GetPixels();


    // starting at px.x -1, px. y + 1
    for (int i = -BrushSize/2; i <= BrushSize/2; ++i) {
      for (int j = -BrushSize/2; j <= BrushSize/2; ++j)
      {
        if (tex.GetPixel((int)pixelUV.x + i, (int)pixelUV.y + j) != null) //if the pixel exists, then we change it
          tex.SetPixel((int)pixelUV.x + i, (int)pixelUV.y + j, Color.black);
      }

    }
    tex.Apply();
  }
  //Sets the base material with a our canvas texture, then removes all our brushes
  void SaveTexture()
  {
    brushCounter = 0;
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



  IEnumerator SaveTextureToFile(Texture2D savedTexture)
  {
    brushCounter = 0;
    string fullPath = System.IO.Directory.GetCurrentDirectory() + "\\UserCanvas\\";
    System.DateTime date = System.DateTime.Now;
    string fileName = "CanvasTexture.png";
    if (!System.IO.Directory.Exists(fullPath))
      System.IO.Directory.CreateDirectory(fullPath);
    var bytes = savedTexture.EncodeToPNG();
    System.IO.File.WriteAllBytes(fullPath + fileName, bytes);
    Debug.Log("<color=orange>Saved Successfully!</color>" + fullPath + fileName);
    yield return null;
  }
}
