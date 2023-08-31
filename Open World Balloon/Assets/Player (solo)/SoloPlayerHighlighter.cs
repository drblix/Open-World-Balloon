using UnityEngine;

public class SoloPlayerHighlighter : MonoBehaviour
{
    private static readonly int ColorProperty = Shader.PropertyToID("_Color");

    [SerializeField] private Color highlightColor;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float castDistance;

    private GameObject _currentObject;
    private Material _currentMatHighlight, _currentMaterialPrev;
    
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
            // is a new object
            if (_currentObject != highlightableObj.gameObject)
            {
                if (_currentMatHighlight != null && _currentMaterialPrev != null)
                {
                    _currentObject.GetComponent<MeshRenderer>().material = _currentMaterialPrev;
                    Destroy(_currentMatHighlight);
                }

                _currentObject = highlightableObj.gameObject;
                
                MeshRenderer meshRenderer = _currentObject.GetComponent<MeshRenderer>();
                _currentMaterialPrev = meshRenderer.sharedMaterial;
                _currentMatHighlight = meshRenderer.material;
                _currentMatHighlight.SetColor(ColorProperty, highlightColor);
                meshRenderer.material = _currentMatHighlight;
            }
        }
        // not looking at any highlightable object
        // clear any previous objects that are highlighted
        else
        {
            if (_currentObject != null)
            {
                _currentObject.GetComponent<MeshRenderer>().material = _currentMaterialPrev;
                Destroy(_currentMatHighlight);
                
                _currentObject = null;
                _currentMaterialPrev = null;
                _currentMatHighlight = null;
            }
        }
    }
}
