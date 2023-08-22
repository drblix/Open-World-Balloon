using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Camera))]
public class SoloPlayerCamera : MonoBehaviour
{
    public static SoloPlayerCamera Singleton;

    private const float MaxLookAngle = 90f;

    [SerializeField] private float mouseSensitivity = 2f;
    
    [SerializeField] private Transform playerTransform;

    [SerializeField] private bool invertMouse = true;

    private float _xRotation = 0f;

    private void Awake()
    {
        Singleton = this;
    }
    
    private void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        if (invertMouse)
            _xRotation += mouseY;
        else
            _xRotation -= mouseY;

        _xRotation = Mathf.Clamp(_xRotation, -MaxLookAngle, MaxLookAngle);
        
        playerTransform.Rotate(Vector3.up * mouseX);

        Vector3 eulerAngles = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(_xRotation, eulerAngles.y, eulerAngles.z);
    }
}
