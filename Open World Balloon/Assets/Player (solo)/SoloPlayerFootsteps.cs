using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloPlayerFootsteps : MonoBehaviour
{
    private Terrain _currentTerrain;
    private TerrainData _currentData;

    private void Start()
    {
        _currentTerrain = TerrainUtils.GetTerrainClosestToPoint(transform.position);
        _currentData = _currentTerrain.terrainData;
        // Debug.Log(_currentData.name);
    }

    private void Update()
    {
        float[] textureMix = GetTextureMixtureAtPosition(transform.position);
        int maxIndex = MaxInd(textureMix);


        Debug.Log("Closest terrain: " + TerrainUtils.GetTerrainClosestToPoint(transform.position).name);
        //Debug.Log("Standing on: " + _currentData.terrainLayers[maxIndex].diffuseTexture.name);
        
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
}
