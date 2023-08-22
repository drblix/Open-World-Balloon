using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SoloPlayerMovement : MonoBehaviour
{
    public static SoloPlayerMovement Singleton;
    
    private Rigidbody _rigidbody;

    [SerializeField] private float maxMovementSpeed = 8f;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float groundCastDistance = .8f;
    [SerializeField] private float jumpForce = 5f;

    private void Awake()
    {
        Singleton = this;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Movement();
        Jumping();
    }

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 curVelocity = _rigidbody.velocity;
        // Vector3 targetVelocity = new Vector3(horizontal, 0f, vertical) * movementSpeed;
        Vector3 targetVelocity = new (horizontal, 0f, vertical);
        float prevMagnitude = Mathf.Clamp01(targetVelocity.magnitude);
        targetVelocity.Normalize();
        
        targetVelocity = transform.TransformDirection(targetVelocity) * movementSpeed;
        targetVelocity *= prevMagnitude;

        // Calculate our change in velocity and clamp the change
        Vector3 change = targetVelocity - curVelocity;
        change.Set(change.x, 0f, change.z);
        
        change = Vector3.ClampMagnitude(change, maxMovementSpeed);

        // Add our force, disregarding the player's mass
        _rigidbody.AddForce(change, ForceMode.VelocityChange);
        
    }
    
    private void Jumping()
    {
        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }
    
    public bool IsMoving() => _rigidbody.velocity.sqrMagnitude > 0.1f;
    private bool IsGrounded() => Physics.Raycast(transform.position, -transform.up, groundCastDistance);
}
