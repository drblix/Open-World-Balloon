using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovingBody : MonoBehaviour
{
    private Rigidbody _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            _rigidbody.AddForce(transform.forward * 5f, ForceMode.VelocityChange);
        }
    }
}
