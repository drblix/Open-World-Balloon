using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, Interactable
{
    [SerializeField] private Transform pivot;
    [SerializeField] private float speed = 3f;
    [SerializeField] private bool openBasedOnPlayer;

    private Transform _playerCamera;
    private Quaternion _closedRotationQuat;
    
    private bool _inAnimation = false;
    private bool _open = false;
    
    private void Start()
    {
        _playerCamera = FindObjectOfType<Camera>().transform;
        _closedRotationQuat = transform.rotation;
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

                Vector3 eulerAngles = _closedRotationQuat.eulerAngles;
                StartCoroutine(dot > 0f
                    ? ToggleDoor(Quaternion.Euler(eulerAngles.x, eulerAngles.y - 90f, eulerAngles.z))
                    : ToggleDoor(Quaternion.Euler(eulerAngles.x, eulerAngles.y + 90f, eulerAngles.z)));
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
            while (pivot.rotation != goalRotation.Value)
            {
                pivot.rotation = Quaternion.Lerp(_closedRotationQuat, goalRotation.Value, timer);

                timer += Time.deltaTime * speed;
                yield return new WaitForEndOfFrame();
            }
        }
        // closing door
        else
        {
            Quaternion startingRotation = pivot.rotation;
            float timer = 0f;
            while (pivot.rotation != _closedRotationQuat)
            {
                pivot.rotation = Quaternion.Lerp(startingRotation, _closedRotationQuat, timer);

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
