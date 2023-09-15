using UnityEngine;
using TMPro;

public class SoloPlayerLabel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objectLabel;

    private void Update()
    {
        //Ray ray = new (playerCamera.position, playerCamera.forward);

        //if (Physics.Raycast(ray, out RaycastHit hit, castDistance, _interactableMask) && hit.collider.TryGetComponent(out Labeled labeledObj) && labeledObj.visible)
        SoloPlayerRaycaster.PlayerRaycast playerRaycastData = SoloPlayerRaycaster.Singleton.GetPlayerRaycastInfo();
        if (playerRaycastData.HitThisFrame && playerRaycastData.TryGetComponentFromHit(out Labeled labeledObj) && labeledObj.visible)
        {
            objectLabel.SetText(labeledObj.label);
        }
        else
        {
            objectLabel.SetText(string.Empty);
        }
    }
}
