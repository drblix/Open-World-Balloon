using UnityEngine;

public class SoloPlayerFootsteps : MonoBehaviour
{
    [SerializeField] private AudioSource footstepSource;

    [SerializeField] private AudioClip[] grassSounds, woodSounds;

    [SerializeField] [Tooltip("Minimum time that must pass before another footstep can be played")] private float minimumWait = .2f;

    private Terrain _currentTerrain;
    private TerrainData _currentData;
    private int _terrainMask;

    private Steppable.Material _belowMaterial;

    private float _stepTimer = 0f;

    private void Start()
    {
        _currentTerrain = TerrainUtils.GetTerrainClosestToPoint(transform.position);
        _currentData = _currentTerrain.terrainData;
        _terrainMask = LayerMask.GetMask("Terrain");
    }

    private void Update()
    {
        if (_stepTimer < minimumWait) _stepTimer += Time.deltaTime;

        // player must be on ground and moving
        if (!SoloPlayerMovement.Singleton.IsMoving() || !SoloPlayerMovement.Singleton.IsGrounded()) return;
        
        GameObject belowObj = SoloPlayerMovement.Singleton.GetGameObjectBelow();
        if (belowObj != null && belowObj.TryGetComponent(out Steppable steppable))
        {
            if (steppable.isTerrain)
            {
                int dominantTerrain = MaxInd(GetTextureMixtureAtPosition(transform.position));
                _belowMaterial = (Steppable.Material) dominantTerrain;
            }
            else
                _belowMaterial = steppable.material;
        }
        else return;

        AudioClip[] clips = new AudioClip[0];
        switch (_belowMaterial)
        {
            case Steppable.Material.Grass:
                clips = grassSounds;
                break;
            case Steppable.Material.Rock:
                clips = grassSounds;
                break;
            case Steppable.Material.Sand:
                clips = grassSounds;
                break;
            case Steppable.Material.Wood:
                clips = woodSounds;
                break;
            default:
                clips = grassSounds;
                break;
        }

        AudioClip randClip = clips[Random.Range(0, clips.Length)];

        if (_stepTimer > minimumWait)
        {
            footstepSource.clip = randClip;
            footstepSource.Play();

            _stepTimer = 0f;
        }

        // Debug.Log("Standing on: " + _belowMaterial.ToString());
        
        /*
        Ray ray = new (transform.position, -transform.up);
        if (SoloPlayerMovement.Singleton.IsMoving() && Physics.Raycast(ray, out RaycastHit hit, SoloPlayerMovement.Singleton.groundCastDistance))
        {
            if (!hit.collider) return;

            if (hit.collider.TryGetComponent(out TerrainCollider terrainCollider))
            {
                TerrainData data = terrainCollider.terrainData;

                float[,,] alphamaps = data.GetAlphamaps(0, 0, data.alphamapWidth, data.alphamapHeight);

                Debug.Log(alphamaps[0, 0, 0]);
            }
        }
        */
    }

    // Length of the array is how many textures are on the provided terrain
    private float[] GetTextureMixtureAtPosition(Vector3 worldPosition)
    {
        // Getting the position on the splatmap relative to the provided world position
        Vector3 terrainPosition = _currentTerrain.GetPosition();
        int mapX = Mathf.FloorToInt((worldPosition.x - terrainPosition.x) / _currentData.size.x * _currentData.alphamapWidth);
        int mapY = Mathf.FloorToInt((worldPosition.z - terrainPosition.z) / _currentData.size.z * _currentData.alphamapHeight);

        float[,,] splatmap = _currentData.GetAlphamaps(mapX, mapY, 1, 1);
        float[] textureMixtures = new float[splatmap.GetUpperBound(2) + 1];

        for (int i = 0; i < textureMixtures.Length; i++)
            textureMixtures[i] = splatmap[0, 0, i];

        return textureMixtures;
    }

    private int MaxInd(float[] arr)
    {
        int maxIndex = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[maxIndex] < arr[i])
                maxIndex = i;
        }

        return maxIndex;
    }

    private bool IsGroundedOnTerrain()
    {
        Ray ray = new(transform.position, -transform.up);
        return Physics.Raycast(ray, SoloPlayerMovement.Singleton.groundCastDistance, _terrainMask);
    }
}
