using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class Render
{
    Scene scene;
    int height;
    int width;
    Camera camera;
    public Color[] colorBuffer;
    public float[] depthBuffer;

    private int GetIndex(int x, int y) { return y * width + x; }
    public Render(int width, int height, Scene s)
    {
        this.width = width;
        this.height = height;
        camera = new Camera(new Vector3(0, 0, 10));
        scene = s;
        colorBuffer = new Color[width * height];
        depthBuffer = new float[width * height];

        ClearBuffer();
    }

    void ClearBuffer()
    {
        for (int j = 0; j < height; j++)
            for (int i = 0; i < width; i++)
            {
                colorBuffer[GetIndex(i, j)] = Color.black;
                depthBuffer[GetIndex(i, j)] = float.MinValue;
            }
    }

    public Color[] GenerateImage()
    {
        ClearBuffer();
        Matrix4x4 view = camera.GetView();
        Matrix4x4 projection = Matrix4x4.Perspective(60, (float)width / (float)height, 1, 100);
        foreach (KeyValuePair<string, Object> o in scene.objects)
        {
            Matrix4x4 model = o.Value.model;
            Matrix4x4 trans = projection * view * model;
            o.Value.GetRasterizedData(trans, width, height, 1, 100);
            int x1, y1, x2, y2;
            o.Value.GetBoundingBox(out x1, out y1, out x2, out y2);
            for (int j = Math.Max(0, y1); j < Math.Min(height, y2); j++)
                for (int i = Math.Max(0, x1); i < Math.Min(width, x2); i++)
                {
                    float depth;
                    if (o.Value.InsideObject(i, j, out depth) && depth > depthBuffer[GetIndex(i, j)])
                    {
                        colorBuffer[GetIndex(i, j)] = o.Value.GetColor(i, j);
                        depthBuffer[GetIndex(i, j)] = depth;
                    }
                }
        }
        return colorBuffer; 
    }
}
