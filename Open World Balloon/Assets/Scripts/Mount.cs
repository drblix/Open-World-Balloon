using UnityEngine;

public class Mount : MonoBehaviour
{
    public Carryable MountedObject
    {
        get { return _mountedObject; }
        private set { _mountedObject = value; }
    }

    [SerializeField] private Transform _mountParent;

    private Carryable _mountedObject;

    public void MountObject(GameObject gameObject)
    {
        Carryable carryable = gameObject.GetComponent<Carryable>();

        gameObject.transform.SetParent(_mountParent);
        gameObject.transform.localPosition = Vector3.zero + carryable.mountOffset;
        gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero) * carryable.mountRotation;

        MountedObject = carryable;
    }

    public void UnmountObject()
    {
        _mountedObject = null;
    }

    public bool ObjectIsMounted() => _mountedObject != null;
}
