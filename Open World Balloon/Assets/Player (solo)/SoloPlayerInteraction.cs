using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class SoloPlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float castDistance = 2.5f;

    private int _interactableMask;

    private void Awake()
    {
        _interactableMask = LayerMask.GetMask("Interactable");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, castDistance, _interactableMask))
            {
                if (hit.collider.TryGetComponent(out Interactable interactable))
                {
                    interactable.Interact();
                    // Vector3 dir = (hit.collider.transform.position - playerCamera.position).normalized;
                    // float dot = Vector3.Dot(dir, hit.collider.transform.forward);

                    // Debug.Log(dot < 0f ? "in front" : "behind");
                }
            }
        }
        
    }
}
