using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoloPlayerHighlighter : MonoBehaviour
{
    private static readonly int ColorProperty = Shader.PropertyToID("_Color");

    [SerializeField] private Color highlightColor;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float castDistance;

    private GameObject _currentObject;
    private Material[] _highlightedMaterials, _previousMaterials;
    
    private int _interactableMask;

    private void Awake()
    {
        _interactableMask = LayerMask.GetMask("Interactable");
    }

    private void Update()
    {
        HighlightRaycast();
    }
    
    // 1. check if hovering over highlightable object, if not goto 1a
    // 2. is the highlightable object a new object?
    // 3. if a new object, unhighlight previous object, highlight new one
    // 1a. check if any highlighted object, if so, unhighlight
    private void HighlightRaycast()
    {
        Ray ray = new(playerCamera.position, playerCamera.forward);
        
        // looking at a highlightable object
        if (Physics.Raycast(ray, out RaycastHit hit, castDistance, _interactableMask) && hit.collider.TryGetComponent(out Highlightable highlightableObj) && highlightableObj.highlightable)
        {
            // a new object has been looked at
            if (_currentObject != highlightableObj.gameObject)
            {
                _currentObject = highlightableObj.gameObject;

                // getting an array of our object's materials before being replaced with highlighted versions
                MeshRenderer renderer = _currentObject.GetComponent<MeshRenderer>();
                _previousMaterials = renderer.sharedMaterials;

                // getting the object's materials and replacing them with highlighted versions, storing them
                // in the highlighted materials array
                Material[] mats = renderer.materials;
                _highlightedMaterials = new Material[mats.Length];
                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i].SetColor(ColorProperty, highlightColor);
                    _highlightedMaterials[i] = mats[i];
                }
            }
        }
        // not looking at any highlightable object
        // destroy all highlight materials associated with previous object
        else
        {
            // checking if the previous object exists
            if (_currentObject != null)
            {
                // if a previous object existed, destroy all highlighted materials that were created
                // and reassign the object's previous materials
                
                MeshRenderer renderer = _currentObject.GetComponent<MeshRenderer>();

                for (int i = 0; i < _highlightedMaterials.Length; i++)
                    Destroy(_highlightedMaterials[i]);

                renderer.materials = _previousMaterials;

                // clearing object
                _currentObject = null;
            }
        }
    }
}
