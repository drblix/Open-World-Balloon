using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class ImpactSounds : MonoBehaviour
{
    [SerializeField] [Tooltip("Minimum impact velocity magnitude for an impact sound to be played")] private float soundThreshold;
    [SerializeField] [Tooltip("The amount of time that must pass before another clip can be played")] private float timerThreshold = .75f;
    [SerializeField] private AudioClip impactClip;

    private AudioSource _impactSource;
    [SerializeField] private float _impactTimer = 0f;


    private void Start()
    {
        _impactSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_impactTimer < timerThreshold) _impactTimer += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_impactTimer < timerThreshold) return;
        _impactTimer = 0f;

        Vector3 impactVelocity = other.relativeVelocity;
        if (impactVelocity.sqrMagnitude >= soundThreshold * soundThreshold)
        {
            _impactSource.clip = impactClip;
            _impactSource.volume = Mathf.Lerp(0f, 1f, (impactVelocity.sqrMagnitude - soundThreshold * soundThreshold) / 100f);
            _impactSource.Play();
        }
    }
}
