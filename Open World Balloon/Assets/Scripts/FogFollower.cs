using UnityEngine;

public class FogFollower : MonoBehaviour
{
    [SerializeField] private Transform fogTransform;
    private Transform _playerTransform;

    private void Start()
    {
        _playerTransform = SoloPlayerMovement.Singleton.transform;
    }

    private void Update()
    {
        fogTransform.position = new (_playerTransform.position.x, 2.5f, _playerTransform.position.z);
    }
}
