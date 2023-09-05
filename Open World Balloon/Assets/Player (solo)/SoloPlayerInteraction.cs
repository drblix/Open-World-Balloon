using UnityEngine;

public class SoloPlayerInteraction : MonoBehaviour
{
    public static SoloPlayerInteraction Singleton;
    
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float castDistance = 2.5f;

    public int interactableMask;

    private void Awake()
    {
        Singleton = this;
        interactableMask = LayerMask.GetMask("Interactable");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, castDistance, interactableMask))
            {
                if (hit.collider.TryGetComponent(out Interactable interactable))
                {
                    interactable.Interact();
                }
            }
        }
        
    }
}
