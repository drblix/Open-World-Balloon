using UnityEngine;

public class SoloPlayerInventory : MonoBehaviour
{
    [SerializeField] private Transform carryLocation;
    
    private Carryable _currentCarryable;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // player can only drop item if not hovering over interactable
        SoloPlayerRaycaster.PlayerRaycast playerRaycastInfo = SoloPlayerRaycaster.Singleton.GetPlayerRaycastInfo();
        if (_currentCarryable != null && Input.GetMouseButtonDown(0))
        {
            // going to need a special behaviour check for things that can be mounted on the balloon
            // plan to add a burner, compass, and maybe some other misc. things; all of which will be mountable
            if (_currentCarryable.isMountable && playerRaycastInfo.HitThisFrame && playerRaycastInfo.TryGetComponentFromHit(out Mount mount))
            {
                mount.MountObject(_currentCarryable.gameObject);
                _currentCarryable = null;
                return;
            }

            if (playerRaycastInfo.HitThisFrame && playerRaycastInfo.TryGetComponentFromHit(out Interactable _)) return;

            // casts a box to check if user is trying to place item in object
            bool insideObject = Physics.CheckBox(_currentCarryable.transform.position,
                _currentCarryable.transform.localScale / 2f,
                _currentCarryable.transform.rotation, 
                ~(1 << 8), 
                QueryTriggerInteraction.Ignore);
            
            if (insideObject) return;
            
            // drop current item being carried
            // reenable physics on object
            Rigidbody carryableBody = _currentCarryable.GetComponent<Rigidbody>();
            carryableBody.isKinematic = false;
            carryableBody.useGravity = true;

            _currentCarryable.transform.SetParent(null);

            _currentCarryable.GetComponent<Collider>().enabled = true;
            _currentCarryable.GetComponent<Rigidbody>().AddForce(_rigidbody.velocity, ForceMode.VelocityChange);

            if (_currentCarryable.carrySource != null)
            {
                _currentCarryable.carrySource.clip = _currentCarryable.carryClip;
                _currentCarryable.carrySource.volume = 1f;
                _currentCarryable.carrySource.Play();
            }

            _currentCarryable = null;
        }
        else if (_currentCarryable == null && Input.GetMouseButtonDown(1) && playerRaycastInfo.HitThisFrame /*Physics.Raycast(ray, out RaycastHit hit, castDistance, SoloPlayerInteraction.Singleton.interactableMask)*/)
        {
            if (playerRaycastInfo.TryGetComponentFromHit(out Carryable carryable))
            {
                PickupObject(carryable);
            }
            else if (playerRaycastInfo.TryGetComponentFromHit(out Mount mount) && mount.ObjectIsMounted())
            {
                PickupObject(mount.MountedObject);
                mount.UnmountObject();
            }
        }
    }

    private void PickupObject(Carryable carryable)
    {
        // pick up item on ground
        Rigidbody carryableBody = carryable.GetComponent<Rigidbody>();
        carryableBody.isKinematic = true;
        carryableBody.useGravity = false;

        carryable.GetComponent<Collider>().enabled = false;

        carryable.transform.SetParent(transform);

        carryable.transform.localRotation = carryable.carryRotation;
        carryable.transform.localPosition = carryLocation.localPosition + carryable.carryOffset;

        _currentCarryable = carryable;

        if (_currentCarryable.carrySource != null)
        {
            _currentCarryable.carrySource.clip = _currentCarryable.carryClip;
            _currentCarryable.carrySource.volume = 1f;
            _currentCarryable.carrySource.Play();
        }
    }

}
