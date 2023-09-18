using UnityEngine;

public class CompassNeedle : MonoBehaviour
{
    [SerializeField] private Vector3 northPoleLocation = Vector3.zero;
    [SerializeField] private float rotationSpeed = 5f;

    private void Update()
    {
        float x = northPoleLocation.x - transform.position.x;
        float z = northPoleLocation.z - transform.position.z;
        Quaternion goalRotation = Quaternion.Euler(0f, Mathf.Atan2(x, z) * Mathf.Rad2Deg, 0f);

        transform.rotation = goalRotation;
        transform.localRotation = Quaternion.Euler(0f, transform.localEulerAngles.y, 0f);
    }
}
