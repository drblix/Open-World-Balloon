using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Labeled : MonoBehaviour
{
    [Tooltip("label to be displayed for player")] public string label;
    [Tooltip("can the label be viewed?")] public bool visible = true;
}
