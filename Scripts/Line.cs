using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Line : Object
{
    Vector4 start;
    Vector4 end;
    Vector4 nstart;
    Vector4 nend;

    public Line(Color color, Vector4 start, Vector4 end) : base(color)
    {
        this.start = start;
        this.end = end;
    }

    public override void GetBoundingBox(out int x1, out int y1, out int x2, out int y2)
    {
        x1 = (int)Math.Min(nstart.x, nend.x);
        x2 = (int)Math.Max(nstart.x, nend.x);
        y1 = (int)Math.Min(nstart.y, nend.y);
        y2 = (int)Math.Max(nstart.y, nend.y);
    }

    public override void GetRasterizedData(Matrix4x4 matrix, int width, int height, float znear, float zfar)
    {
        nstart = model * start;
        nend = model * end;
    }

    public override bool InsideObject(int x, int y, out float depth)
    {
        depth = 200;
        float threshold = 2;
        float k = (float)(nend.y - nstart.y) / (float)(nend.x - nstart.x);
        float distance = (float)(Math.Abs(k * (x - nstart.x) - y + nstart.y) / Math.Sqrt(1 + Math.Pow(k, 2)));
        if (distance > threshold)
            return false;
        return true;
    }

    public override Color GetColor(int x, int y)
    {
        float threshold = 2;
        float k = (float)(nend.y - nstart.y) / (float)(nend.x - nstart.x);
        float distance = (float)(Math.Abs(k * (x - nstart.x) - y + nstart.y) / Math.Sqrt(1 + Math.Pow(k, 2)));
        return (threshold - distance) / threshold * color;
    }
}
