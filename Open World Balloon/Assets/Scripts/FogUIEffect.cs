using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FogUIEffect : MonoBehaviour
{
    [SerializeField] private Image fogImage;
    [SerializeField] private float fogHeight = 2.4f, heightThreshold = 4.8f;
    private Transform _playerTransform;


    private void Start()
    {
        _playerTransform = SoloPlayerMovement.Singleton.transform;
    }

    private void Update()
    {
        float newAlpha = 1f - Mathf.InverseLerp(fogHeight, heightThreshold, _playerTransform.position.y);
        fogImage.color = new Color(0.443137f, 0.443137f, 0.443137f, newAlpha);
    }
}
