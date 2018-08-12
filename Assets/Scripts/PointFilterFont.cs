using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PointFilterFont : MonoBehaviour
{
    public Font font;

    [ContextMenu("Apply")]
    public void Apply()
    {
        font.material.mainTexture.filterMode = FilterMode.Point;
    }
}
