/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package ccjavaserver;

/**
 *
 * @author alist
 */
public class FeatureFrame {
    public static int Width;
    public static int Height;
    public static byte[] Pixels;
    
    FeatureFrame(int width, int height, byte pixels[])  {
        Width = width;
        Height = height;
        for (int i = 0 ; i <= width * height; ++i) {
        Pixels[i] = pixels[i];
    }
    };
}
