using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Camera
{
    Vector3 position;
    Vector3 front = Vector3.back;
    Vector3 up = Vector3.up;
    Vector3 right;
    Vector3 worldUp;

    float yaw;
    float pitch = 0.0f;
    float movementSpeed;
    float mouseSensitivity;
    float zoom;

    public Camera(Vector3 pos)
    {
        position = pos;
    }
    
    public Matrix4x4 GetView()
    {
        Matrix4x4 view = Matrix4x4.identity;
        view.m23 = -position.z;
        return view;
    }

    //void Update()
    //{
    //    float velocity = movementSpeed * Time.deltaTime;
    //    if (Input.GetKey(KeyCode.W))
    //        position += front * velocity;
    //    if (Input.GetKey(KeyCode.S))
    //        position -= front * velocity;
    //    if (Input.GetKey(KeyCode.A))
    //        position -= right * velocity;
    //    if (Input.GetKey(KeyCode.D))
    //        position += right * velocity;
    //}

    public void ProcessMouseMovement(float xoffset, float yoffset, bool constrainPitch = true)
    {
        yaw += xoffset;
        pitch += yoffset;

        if (constrainPitch)
        {
            if (pitch > 89.0f)
                pitch = 89.0f;
            if (pitch < -89.0f)
                pitch = -89.0f;
        }

        updateCameraVectors();
    }

    public void ProcessMouseScroll(float yoffset)
    {
        zoom -= (float)yoffset;
        if (zoom < 1.0f)
            zoom = 1.0f;
        if (zoom > 45.0f)
            zoom = 45.0f;
    }

    void updateCameraVectors()
    {
        Vector3 tfront;
        float ryaw = yaw * (float)(Math.PI / 180);
        float rpitch = pitch * (float)(Math.PI / 180);
        tfront.x = (float)(Math.Cos(ryaw) *(float)Math.Cos(rpitch));
        tfront.y = (float)Math.Sin(rpitch);
        tfront.z = (float)(Math.Sin(ryaw) *(float)Math.Cos(rpitch));
        front = tfront.normalized;
        right = Vector3.Cross(front, worldUp).normalized;
        up = Vector3.Cross(right, front).normalized; 
    }
}
