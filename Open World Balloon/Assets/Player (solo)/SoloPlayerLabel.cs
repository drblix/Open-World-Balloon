using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoloPlayerLabel : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private TextMeshProUGUI objectLabel;
    [SerializeField] private float castDistance = 2.5f;

    private int _interactableMask;

    private void Awake()
    {
        _interactableMask = LayerMask.GetMask("Interactable");
    }

    private void Update()
    {
        Ray ray = new (playerCamera.position, playerCamera.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, castDistance, _interactableMask) && hit.collider.TryGetComponent(out Labeled labeledObj))
        {
            objectLabel.SetText(labeledObj.GetLabel());
        }
        else
        {
            objectLabel.SetText(string.Empty);
        }
    }
}
