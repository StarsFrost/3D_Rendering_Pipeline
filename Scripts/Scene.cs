using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene
{
    public Dictionary<string, Object> objects;
    public List<string> ObjectNames
    {
        get
        {
            List<string> l = new List<string>();
            foreach (KeyValuePair<string, Object> pair in objects) { l.Add(pair.Key); }
            return l;
        }
    }
    public Scene()
    {
        objects = new Dictionary<string, Object>();
    }

    public void AddObject(string key, Object obj)
    {
        objects[key] = obj;
    }

    public void RemoveObject(string key)
    {
        objects.Remove(key);
    }

    public void translate(int value, string key, float x, float y, float z)
    {
        objects[key].Transform(value, x, y, z);
    }
}
