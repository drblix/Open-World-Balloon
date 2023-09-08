using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, Interactable
{
    [SerializeField] private Transform pivot;
    [SerializeField] private float speed = 3f;
    [SerializeField] private bool openBasedOnPlayer;

    [SerializeField] private Vector3 doorAxis;

    private Transform _playerCamera;

    private Quaternion _startingRotation;
    // private Quaternion _closedRotationQuat;
    
    private bool _inAnimation = false;
    private bool _open = false;
    
    private void Start()
    {
        _playerCamera = FindObjectOfType<Camera>().transform;
        _startingRotation = pivot.localRotation;
        // _closedRotationQuat = transform.localRotation;
    }

    public void Interact()
    {
        if (_inAnimation) return;
        
        if (!_open)
        {
            // used for balloon doors
            // open direction depends on where the player is standing in relation to the door (behind or in front)
            if (openBasedOnPlayer)
            {
                Vector3 dir = (_playerCamera.position - transform.position).normalized;
                float dot = Vector3.Dot(dir, transform.forward);
                
                StartCoroutine(dot > 0f
                    // in front
                    ? ToggleDoor(_startingRotation * Quaternion.AngleAxis(-90f, doorAxis)) // ToggleDoor(Quaternion.Euler(eulerAngles.x, eulerAngles.y - 89f, eulerAngles.z))
                    // behind
                    : ToggleDoor(_startingRotation * Quaternion.AngleAxis(90f, doorAxis))); // ToggleDoor(Quaternion.Euler(eulerAngles.x, eulerAngles.y + 89f, eulerAngles.z)));
            }

            _open = true;
        }
        else
        {
            StartCoroutine(ToggleDoor(null));
            _open = false;
        }
    }

    private IEnumerator ToggleDoor(Quaternion? goalRotation)
    {
        _inAnimation = true;
        
        if (goalRotation != null)
        {
            float timer = 0f;
            while (pivot.localRotation != goalRotation.Value)
            {
                pivot.localRotation = Quaternion.Lerp(_startingRotation, goalRotation.Value, timer);

                timer += Time.deltaTime * speed;
                yield return new WaitForEndOfFrame();
            }
        }
        // closing door
        else
        {
            Quaternion startingRotation = pivot.localRotation;
            float timer = 0f;
            while (pivot.localRotation != _startingRotation)
            {
                pivot.localRotation = Quaternion.Lerp(startingRotation, _startingRotation, timer);

                timer += Time.deltaTime * speed;
                yield return new WaitForEndOfFrame();
            }
        }

        _inAnimation = false;
    }

    public bool InRange()
    {
        throw new System.NotImplementedException();
    }
}
