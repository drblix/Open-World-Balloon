using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BasketPhysics : MonoBehaviour
{
    [SerializeField] private Rigidbody basketRigidbody;

    private readonly List<Rigidbody> _touchingBodies = new();
    
    // TODO:
    // somehow sync basket angular velocity w/ player
    // current implementation just rotates towards center slowly
    // https://www.desmos.com/calculator/n6mcuzlizi
    private void FixedUpdate()
    {
        if (_touchingBodies.Count == 0) return;
        
        foreach (Rigidbody body in _touchingBodies)
        {
            if (body.isKinematic || !body.CompareTag("Player")) continue;
            
            Vector3 relativeVelocity = basketRigidbody.GetPointVelocity(body.transform.position);
            relativeVelocity.y = 0f;
            
            body.AddForce(relativeVelocity, ForceMode.VelocityChange);
        }
    }

    private static Vector3 RotatePointBy(Vector3 point, float radians)
    {
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        point.x = cos * point.x - sin * point.z;
        point.z = sin * point.x + cos * point.z;
        
        return point;
    }

    private void OnTriggerEnter(Collider other)
    {
        _touchingBodies.Add(other.GetComponent<Rigidbody>());
    }

    private void OnTriggerExit(Collider other)
    {
        _touchingBodies.Remove(other.GetComponent<Rigidbody>());
    }
}
