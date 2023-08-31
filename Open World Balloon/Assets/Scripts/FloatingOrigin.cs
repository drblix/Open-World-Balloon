using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
public class FloatingOrigin : MonoBehaviour
{
    [SerializeField] [Tooltip("the units the player must pass before all objects are translated")] 
    private float threshold = 100f;
    
    // optimally, this should be late update, but there's a chance rigidbody objects get flung backwards if we use it
    // using fixed update resolves this problem
    private void FixedUpdate()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.y = 0f;
        
        
        if (cameraPosition.sqrMagnitude > threshold * threshold)
        {
            GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject obj in rootObjects)
                obj.transform.position -= cameraPosition;
            
            Physics.SyncTransforms();
        }
    }
}
