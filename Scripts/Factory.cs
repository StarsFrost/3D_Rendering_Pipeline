using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface IFactory
{   
    public Object CreateObject(Vector4[] data, Color color);
}

public class CubeFactory : IFactory
{
    public Object CreateObject(Vector4[] data, Color color)
    {
        return new Cube(color, data[0], data[1]);
    }
}

public class TriangleFactory : IFactory
{
    public Object CreateObject(Vector4[] data, Color color)
    {
        return new Triangle(color, data);
    }
}

public class CircleFactory : IFactory
{
    public Object CreateObject(Vector4[] data, Color color)
    {
        return new Circle(color, data[0], data[0].z);
    }
}

public class LineFactory : IFactory
{
    public Object CreateObject(Vector4[] data, Color color)
    {
        return new Line(color, new Vector4(data[0].x, data[0].y, 200, 1), new Vector4(data[1].x, data[1].y, 200, 1));
    }
}