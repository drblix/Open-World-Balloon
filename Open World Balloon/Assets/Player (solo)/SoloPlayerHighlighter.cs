using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloPlayerHighlighter : MonoBehaviour
{
    [SerializeField] private GameObject cubeHighlight;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float castDistance;

    private Highlightable _currentHighlightable;

    private int _interactableMask;

    private void Awake()
    {
        _interactableMask = LayerMask.GetMask("Interactable");
    }

    private void Update()
    {
        Ray ray = new(playerCamera.position, playerCamera.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, castDistance, _interactableMask) && hit.collider.TryGetComponent(out Highlightable highlightableObj))
            _currentHighlightable = highlightableObj;
        else
            _currentHighlightable = null;

        if (_currentHighlightable != null)
        {
            cubeHighlight.SetActive(true);
            cubeHighlight.transform.SetParent(_currentHighlightable.transform);

            cubeHighlight.transform.localPosition = Vector3.zero;
            cubeHighlight.transform.localRotation = Quaternion.Euler(Vector3.zero);
            cubeHighlight.transform.localScale = Vector3.one * 1.04f;
        }
        else
        {
            cubeHighlight.SetActive(false);
            cubeHighlight.transform.SetParent(null);
        }
    }
}
