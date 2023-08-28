using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LineUpdater : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private Transform startTransform, endTransform;

    private void Update()
    {
        if (lineRenderer == null || startTransform == null || endTransform == null) return;
        
        lineRenderer.SetPosition(0, startTransform.position);
        lineRenderer.SetPosition(1, endTransform.position);
    }
}
