using UnityEngine;

public class SoloPlayerInventory : MonoBehaviour
{
    [SerializeField] private Transform carryLocation;
    [SerializeField] private float castDistance = 2.5f;
    
    private Carryable _currentCarryable;


    private void Update()
    {
        Ray ray = new (SoloPlayerCamera.Singleton.transform.position, SoloPlayerCamera.Singleton.transform.forward);
        
        // player can only drop item if not hovering over interactable
        if (_currentCarryable != null && Input.GetMouseButtonDown(0) && !Physics.Raycast(ray, castDistance, SoloPlayerInteraction.Singleton.interactableMask))
        {
            // casts a box to check if user is trying to place item in object
            bool insideObject = Physics.CheckBox(_currentCarryable.transform.position,
                _currentCarryable.transform.localScale / 2f,
                _currentCarryable.transform.rotation);
            
            if (insideObject) return;
            
            // drop current item being carried
            // reenable physics on object
            Rigidbody carryableBody = _currentCarryable.GetComponent<Rigidbody>();
            carryableBody.isKinematic = false;
            carryableBody.useGravity = true;

            _currentCarryable.GetComponent<Collider>().enabled = true;
            
            _currentCarryable.transform.SetParent(null);

            _currentCarryable = null;
        }
        else if (_currentCarryable == null && Input.GetMouseButtonDown(1) && Physics.Raycast(ray, out RaycastHit hit, castDistance, SoloPlayerInteraction.Singleton.interactableMask))
        {
            if (hit.collider.TryGetComponent(out Carryable carryable))
            {
                // pick up item on ground
                Rigidbody carryableBody = carryable.GetComponent<Rigidbody>();
                carryableBody.isKinematic = true;
                carryableBody.useGravity = false;

                carryable.GetComponent<Collider>().enabled = false;
                
                carryable.transform.SetParent(transform);

                carryable.transform.localPosition = carryLocation.localPosition - carryable.carryPivot.localPosition;
                carryable.transform.localRotation = Quaternion.Euler(Vector3.zero);

                _currentCarryable = carryable;
            }
        }
    }
    
}
