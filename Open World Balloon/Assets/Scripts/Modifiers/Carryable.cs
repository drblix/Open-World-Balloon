using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Carryable : MonoBehaviour
{
    public Vector3 carryOffset;
    public AudioSource carrySource;
    public AudioClip carryClip;
}
