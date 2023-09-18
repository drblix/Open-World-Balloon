using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Carryable : MonoBehaviour
{
    public Vector3 carryOffset;
    public Quaternion carryRotation;
    public AudioSource carrySource;
    public AudioClip carryClip;

    [Header("Mount Settings")]
    public bool isMountable = false;
    public Vector3 mountOffset;
    public Quaternion mountRotation;
}
