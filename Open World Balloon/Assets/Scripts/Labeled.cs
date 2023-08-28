using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Labeled : MonoBehaviour
{
    [Tooltip("label to be displayed for player")] [SerializeField] private string label;

    public string GetLabel() => label;
}
