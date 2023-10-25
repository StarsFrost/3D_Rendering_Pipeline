using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Model
{
    Render render;
    public Scene scene;
    public Texture2D texture;
    public event UnityAction<Model> handler;

    private static Model data;
    public static Model Data
    {
        get
        {
            if (data == null)
            {
                data = new Model();
            }
            return data;
        }
    }
    public void Init(int width, int height)
    {
        scene = new Scene();
        render = new Render(width, width, scene);
        texture = new Texture2D(width, height);

        UpdateImage();
    }

    public void AddObject(string key, Object obj)
    {
        scene.AddObject(key, obj);
        UpdateImage();
    }

    public void RemoveObject(string key)
    {
        scene.RemoveObject(key);
        UpdateImage();
    }

    public void UpdateImage()
    {
        Color[] colorData = render.GenerateImage();
        texture.SetPixels(colorData);
        texture.Apply();
        handler?.Invoke(this);
    }

    public void Transform(string key, int value, float x, float y, float z)
    {
        scene.objects[key].Transform(value, x, y, z);
        UpdateImage();
    }
}
