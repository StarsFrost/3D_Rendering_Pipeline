using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : Object
{
    public float radius;
    public Vector4 center;
    public Circle(Color color, Vector4 c, float r) : base(color)
    {
        center = new Vector4(c.x, c.y, 100, 1);
        radius = r;
    }
    public override void GetBoundingBox(out int x1, out int y1, out int x2, out int y2)
    {
        x1 = (int)(center.x - radius - 1);
        x2 = (int)(center.x + radius + 1);
        y1 = (int)(center.y - radius - 1);
        y2 = (int)(center.y + radius + 1);
    }

    public override void GetRasterizedData(Matrix4x4 matrix, int width, int height, float znear, float zfar)
    {
        return;
    }

    public override bool InsideObject(int x, int y, out float depth)
    {
        depth = 100;
        if (System.Math.Pow(x - center.x, 2) + System.Math.Pow(y - center.y, 2) > (radius * radius))
            return false;
        return true;
    }

    public override void Transform(int value, float x, float y, float z)
    {
        switch (value)
        {
            case 0:
                center = Matrix4x4.Translate(new Vector3(x, y, z)) * center;
                break;
            case 2:
                radius *= x;
                break;
            default:
                break;
        }
    }
}
