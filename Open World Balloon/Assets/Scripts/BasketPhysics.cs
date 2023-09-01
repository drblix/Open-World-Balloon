using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketPhysics : MonoBehaviour
{
    [SerializeField] private Rigidbody thisBody;

    private Rigidbody _playerBody;

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.G))
        {
            thisBody.MovePosition(thisBody.position + thisBody.transform.right);
        }

        if (_playerBody == null) return;

        _playerBody.MovePosition(_playerBody.position + thisBody.velocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerBody = other.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerBody = null;
        }
    }
}
