using UnityEngine;

public class Steppable : MonoBehaviour
{
    public enum Material
    {
        Grass,
        Rock,
        Sand,
        Wood
    }

    public bool isTerrain = false;
    public Material material;
}
