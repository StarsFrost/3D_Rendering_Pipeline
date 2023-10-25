using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Triangle : Object
{
    Vector4[] vertices;
    Vector4[] transformedVertices;
    public Triangle(Color color, Vector4[] ver) : base(color)
    { 
        vertices = ver;
        transformedVertices = new Vector4[3];
    }

    public override void GetBoundingBox(out int x1, out int y1, out int x2, out int y2)
    {
        x1 = y1 = int.MaxValue;
        x2 = y2 = int.MinValue;
        foreach (var v in transformedVertices)
        {
            x1 = Math.Min(x1, (int)v.x - 1);
            y1 = Math.Min(y1, (int)v.y - 1);
            x2 = Math.Max(x2, (int)v.x + 1);
            y2 = Math.Max(y2, (int)v.y + 1);
        }
    }

    public override void GetRasterizedData(Matrix4x4 matrix, int width, int height, float znear, float zfar)
    {
        for (int i = 0; i < 3; i++)
        {
            transformedVertices[i] = matrix * vertices[i];
            transformedVertices[i].x /= transformedVertices[i].w;
            transformedVertices[i].y /= transformedVertices[i].w;
            transformedVertices[i].z /= transformedVertices[i].w;
            transformedVertices[i].x = 0.5f * (float)width * (transformedVertices[i].x + 1.0f); 
            transformedVertices[i].y = 0.5f * (float)width * (transformedVertices[i].y + 1.0f);
            transformedVertices[i].z = transformedVertices[i].z * ((zfar - znear) / 2.0f) + (zfar + znear) / 2.0f;
        }
    }

    public override bool InsideObject(int x, int y, out float depth)
    {
        depth = float.MinValue;
        if (!insideTriangle(x, y))
            return false;
        float alpha, beta, gamma;
        computeBarycentric2D(x, y, out alpha, out beta, out gamma);
        Vector4[] v = transformedVertices;
        float Z = 1.0f / (alpha / v[0].w + beta / v[1].w + gamma / v[2].w);
        float zp = alpha * v[0].z / v[0].w + beta * v[1].z / v[1].w + gamma * v[2].z / v[2].w;
        depth = zp * Z;
        return true;

    }

    void computeBarycentric2D(float x, float y, out float alpha, out float beta, out float gamma)
    {
        Vector4[] v = transformedVertices; 
        alpha = (x * (v[1].y - v[2].y) + (v[2].x - v[1].x) * y + v[1].x * v[2].y - v[2].x * v[1].y) / (v[0].x * (v[1].y - v[2].y) + (v[2].x - v[1].x) * v[0].y + v[1].x * v[2].y - v[2].x * v[1].y);
        beta = (x * (v[2].y - v[0].y) + (v[0].x - v[2].x) * y + v[2].x * v[0].y - v[0].x * v[2].y) / (v[1].x * (v[2].y - v[0].y) + (v[0].x - v[2].x) * v[1].y + v[2].x * v[0].y - v[0].x * v[2].y);
        gamma = (x * (v[0].y - v[1].y) + (v[1].x - v[0].x) * y + v[0].x * v[1].y - v[1].x * v[0].y) / (v[2].x * (v[0].y - v[1].y) + (v[1].x - v[0].x) * v[2].y + v[0].x * v[1].y - v[1].x * v[0].y);
    }

    public bool insideTriangle(int x, int y)
    {
        Vector3[] v = new Vector3[3];
        for (int i = 0; i < 3; i++)
            v[i] = new Vector3(transformedVertices[i].x, transformedVertices[i].y, 1);
        Vector3 f0 = Vector3.Cross(v[1], v[0]);
        Vector3 f1 = Vector3.Cross(v[2], v[1]);
        Vector3 f2 = Vector3.Cross(v[0], v[2]);
        Vector3 p = new Vector3(x, y, 1);
        if ((Vector3.Dot(p, f0) * Vector3.Dot(f0, v[2]) > 0) && (Vector3.Dot(p, f1) * Vector3.Dot(f1, v[0]) > 0) && (Vector3.Dot(p, f2) * Vector3.Dot(f2, v[1]) > 0))
            return true;
        return false;
    }

}
