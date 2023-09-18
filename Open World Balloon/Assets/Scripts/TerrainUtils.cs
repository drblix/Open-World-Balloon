using UnityEngine;

public static class TerrainUtils
{
    /// <summary>
    /// Gets the terrain closest to the provided point
    /// </summary>
    public static Terrain GetTerrainClosestToPoint(Vector3 point)
    {
        Terrain[] activeTerrains = Terrain.activeTerrains;
        Terrain closestTerrain = activeTerrains[0];
        float shortestDistance = (closestTerrain.GetCenterPosition() - point).sqrMagnitude;

        foreach (Terrain terrain in activeTerrains)
        {
            float thisDistance = (terrain.GetCenterPosition() - point).sqrMagnitude;
            if (thisDistance < shortestDistance)
            {
                closestTerrain = terrain;
                shortestDistance = thisDistance;

                if (shortestDistance < 262140f) break;
            }
        }

        return closestTerrain;
    }

    /// <summary>
    /// Gets the center position of the terrain in world space
    /// </summary>
    public static Vector3 GetCenterPosition(this Terrain terrain)
    {
        Vector3 originPosition = terrain.GetPosition();
        Vector3 terrainSize = terrain.terrainData.size;
        Vector3 centerPosition = new (originPosition.x + terrainSize.x / 2f, originPosition.y, originPosition.z + terrainSize.z / 2f);
        
        return centerPosition;
    }
    
}
