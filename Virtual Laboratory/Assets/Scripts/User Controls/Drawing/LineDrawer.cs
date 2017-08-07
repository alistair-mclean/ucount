using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LineDrawer {
  
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
