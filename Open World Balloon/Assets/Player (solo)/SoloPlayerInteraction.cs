using UnityEngine;

public class SoloPlayerInteraction : MonoBehaviour
{
    public static SoloPlayerInteraction Singleton;
    [HideInInspector] public int interactableMask;

    private void Awake()
    {
        Singleton = this;
        interactableMask = LayerMask.GetMask("Interactable");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            //if (Physics.Raycast(ray, out RaycastHit hit, castDistance, interactableMask))
            SoloPlayerRaycaster.PlayerRaycast playerRaycastInfo = SoloPlayerRaycaster.Singleton.GetPlayerRaycastInfo();
            if (playerRaycastInfo.HitThisFrame && playerRaycastInfo.TryGetComponentFromHit(out Interactable interactable))
            {
                interactable.Interact();
            }
        }
        
    }
}
