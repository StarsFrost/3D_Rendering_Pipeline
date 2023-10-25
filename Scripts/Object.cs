using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object
{
    public Matrix4x4 model;
    public Color color;
    
    public Object(Color color)
    {
        this.color = color;
        model = Matrix4x4.identity;
    }
    public virtual void Transform(int value, float x, float y, float z) 
    {
        Matrix4x4 m = Matrix4x4.identity;
        switch (value)
        {
            case 0:
                m = Matrix4x4.Translate(new Vector3(x, y, z));
                break;
            case 1:
                Quaternion rotation = Quaternion.Euler(x, y, z);
                m = Matrix4x4.Rotate(rotation);
                break;
            case 2:
                m = Matrix4x4.Scale(new Vector3(x, y, z));
                break;
            default:
                break;
        }
        model = m * model;
    }

    public virtual Color GetColor(int x, int y)
    {
        return color;
    }
    public abstract void GetRasterizedData(Matrix4x4 matrix, int width, int height, float znear, float zfar);

    public abstract void GetBoundingBox(out int x1, out int y1, out int x2, out int y2);

    public abstract bool InsideObject(int x, int y, out float depth);
}
