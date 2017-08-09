using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LineDrawer {
  
  public static void DrawLine(Texture2D tex, int brushSize)
  {
    Vector2 startPosition;
    Vector2 endPosition;
    bool touchEnded = false; // touch has ended
    if (Input.GetTouch(0).phase == TouchPhase.Began)
    {
      startPosition = Input.GetTouch(0).position;
//      Debug.Log("AAAAA Touchphase began!");
    }

    if (Input.GetTouch(0).phase == TouchPhase.Moved)
    {
//      Debug.Log("BBBBB Touchphase moved!");
    }

    if (Input.touchCount == 0)
    {
      touchEnded = true;
    }
    else
      touchEnded = false;

    if (Input.GetTouch(0).phase == TouchPhase.Ended)
    {
      Debug.Log("CCCCC Touchphase ended!!!!!!!!!!");
      endPosition = Input.GetTouch(0).position;
    }
    
  }

  public static void DrawLine(Texture2D tex, Vector2 endPos, Vector2 startPos, Color col)
  {
    int x0 = (int)startPos.x;
    int y0 = (int)startPos.y;
    int x1 = (int)endPos.x;
    int y1 = (int)endPos.y;

    int dy = (int)(y1 - y0);
    int dx = (int)(x1 - x0);
    int stepx, stepy;

    if (dy < 0) { dy = -dy; stepy = -1; }
    else { stepy = 1; }
    if (dx < 0) { dx = -dx; stepx = -1; }
    else { stepx = 1; }
    dy <<= 1;
    dx <<= 1;

    float fraction = 0;

    tex.SetPixel(x0, y0, col);
    if (dx > dy)
    {
      fraction = dy - (dx >> 1);
      while (Mathf.Abs(x0 - x1) > 1)
      {
        if (fraction >= 0)
        {
          y0 += stepy;
          fraction -= dx;
        }
        x0 += stepx;
        fraction += dy;
        tex.SetPixel(x0, y0, col);
      }
    }
    else
    {
      fraction = dx - (dy >> 1);
      while (Mathf.Abs(y0 - y1) > 1)
      {
        if (fraction >= 0)
        {
          x0 += stepx;
          fraction -= dy;
        }
        y0 += stepy;
        fraction += dx;
        tex.SetPixel(x0, y0, col);
      }
    }
  }
}
