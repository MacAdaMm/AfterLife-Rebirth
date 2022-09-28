using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerMaskExtension
{
    public static bool IsInLayerMask(this GameObject obj, LayerMask mask)
    {
        return ((mask.value & (1 << obj.layer)) > 0);
    }
}
