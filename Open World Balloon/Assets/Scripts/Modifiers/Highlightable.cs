using UnityEngine;

public class Highlightable : MonoBehaviour
{
    [Tooltip("can the object be highlighted?")] public bool highlightable = true;
    [Tooltip("renderer whose materials are to be highlighted")] public MeshRenderer renderer;
}
