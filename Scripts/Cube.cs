using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Cube : Object
{
    public Triangle[] triangles;

    public Cube(Color color, Vector3 x, Vector3 y) : base(color)
    {
        triangles = new Triangle[12];
        Vector4 v1 = new Vector4(x.x, x.y, x.z, 1);
        Vector4 v2 = new Vector4(y.x, x.y, x.z, 1);
        Vector4 v3 = new Vector4(y.x, y.y, x.z, 1);
        Vector4 v4 = new Vector4(x.x, y.y, x.z, 1);
        Vector4 v5 = new Vector4(x.x, x.y, y.z, 1);
        Vector4 v6 = new Vector4(y.x, x.y, y.z, 1);
        Vector4 v7 = new Vector4(y.x, y.y, y.z, 1);
        Vector4 v8 = new Vector4(x.x, y.y, y.z, 1);
        triangles[0] = new Triangle(color, new Vector4[3] { v1, v2, v3 });
        triangles[1] = new Triangle(color, new Vector4[3] { v1, v3, v4 });
        triangles[2] = new Triangle(color, new Vector4[3] { v2, v6, v7 });
        triangles[3] = new Triangle(color, new Vector4[3] { v2, v7, v3 });
        triangles[4] = new Triangle(color, new Vector4[3] { v5, v6, v7 });
        triangles[5] = new Triangle(color, new Vector4[3] { v5, v7, v8 });
        triangles[6] = new Triangle(color, new Vector4[3] { v5, v1, v4 });
        triangles[7] = new Triangle(color, new Vector4[3] { v5, v4, v8 });
        triangles[8] = new Triangle(color, new Vector4[3] { v1, v2, v6 });
        triangles[9] = new Triangle(color, new Vector4[3] { v1, v6, v5 });
        triangles[10] = new Triangle(color, new Vector4[3] { v4, v3, v7 });
        triangles[11] = new Triangle(color, new Vector4[3] { v4, v7, v8 });

    }
    public override void GetBoundingBox(out int x1, out int y1, out int x2, out int y2)
    {
        x1 = y1 = int.MaxValue;
        x2 = y2 = int.MinValue;
        foreach (Triangle triangle in triangles)
        {
            int tx1, ty1, tx2, ty2;
            triangle.GetBoundingBox(out tx1, out ty1, out tx2, out ty2);
            x1 = Math.Min(x1, tx1);
            y1 = Math.Min(y1, ty1);
            x2 = Math.Max(x2, tx2);
            y2 = Math.Max(y2, ty2);
        }
    }

    public override void GetRasterizedData(Matrix4x4 matrix, int width, int height, float znear, float zfar)
    {
        foreach (var tri in triangles)
        {
            tri.GetRasterizedData(matrix, width, height, znear, zfar);
        }
    }

    public override bool InsideObject(int x, int y, out float depth)
    {
        depth = float.MinValue;
        bool inside = false;
        foreach (Triangle triangle in triangles)
        {
            float tdepth;
            if (triangle.InsideObject(x, y, out tdepth))
            {
                inside = true;
                depth = Math.Max(depth, tdepth);
            }
        }
        return inside;
    }

    public override void Transform(int value, float x, float y, float z)
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
}
