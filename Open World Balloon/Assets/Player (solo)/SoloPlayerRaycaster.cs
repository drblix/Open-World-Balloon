using UnityEngine;

/// <summary>
/// Universal handler for player raycasting
/// </summary>
public class SoloPlayerRaycaster : MonoBehaviour
{
    public static SoloPlayerRaycaster Singleton;

    
    /// <summary>
    /// Did the raycast hit an object this frame?
    /// </summary>
    public bool HitThisFrame
    {
        get
        {
            return _hitThisFrame;
        }
    }

    public RaycastHit RayHit
    {
        get
        {
            return _rayHit;
        }
    }

    public GameObject HitGameObject
    {
        get
        {
            if (_hitThisFrame)
                return _rayHit.collider.gameObject;
            else
                return null;
        }
    }
    

    /// <summary>
    /// Same as "RaycastHit.distance" but retrieves the distance squared, skipping the square root method call
    /// </summary>
    public float HitSqrDistance
    {
        get
        {
            if (!_hitThisFrame) throw new System.Exception("Cannot fetch member \"HitSqrDistance\" when there has been no raycast hit");

            return (_rayHit.point - cameraTransform.position).sqrMagnitude;
        }
    }

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float castDistance;

    private int _interactableMask;
    private bool _hitThisFrame = false;
    private RaycastHit _rayHit;

    /*
    public bool TryGetComponentFromHit<T>(out T comp)
    {
        if (!_hitThisFrame)
        {
            comp = default;
            return false;
        }

        if (HitGameObject.TryGetComponent(out T component))
        {
            comp = component;
            return true;
        }
        else
        {
            comp = default;
            return false;
        }
    }
    */

    public PlayerRaycast GetPlayerRaycastInfo()
    {
        return new PlayerRaycast(_hitThisFrame, _rayHit, (_rayHit.point - cameraTransform.position).sqrMagnitude);
    }

    // public bool HitInRangeOf(float dist) => HitSqrDistance < dist * dist;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        _interactableMask = LayerMask.GetMask("Interactable");
    }

    private void Update()
    {
        Ray ray = new (cameraTransform.position, cameraTransform.forward);
        _hitThisFrame = Physics.Raycast(ray, out _rayHit, castDistance, _interactableMask);
    }

    public struct PlayerRaycast
    {
        public bool HitThisFrame
        {
            get
            {
                return _hitThisFrame;
            }
        }

        public RaycastHit PlayerRayHit
        {
            get
            {
                return _raycastHit;
            }
        }

        public float SqrDistance
        {
            get
            {
                return _sqrDistance;
            }
        }

        public GameObject HitGameObject
        {
            get
            {
                if (_raycastHit.collider)
                    return _raycastHit.collider.gameObject;
                else
                    return null;
            }
        }

        private bool _hitThisFrame;
        private RaycastHit _raycastHit;
        private float _sqrDistance;

        public PlayerRaycast(bool hitThisFrame, RaycastHit raycastHit, float sqrDistance)
        {
            _hitThisFrame = hitThisFrame;
            _raycastHit = raycastHit;
            _sqrDistance = sqrDistance;
        }

        public bool TryGetComponentFromHit<T>(out T comp)
        {
            if (!HitThisFrame || HitGameObject == null)
            {
                Debug.LogWarning("attempted to get component from a null object");
                comp = default;
                return false;
            }

            if (HitGameObject.TryGetComponent(out T component))
            {
                comp = component;
                return true;
            }
            else
            {
                comp = default;
                return false;
            }
        }

        public readonly bool HitInRangeOf(float dist) => _sqrDistance < dist * dist;
    }
}
