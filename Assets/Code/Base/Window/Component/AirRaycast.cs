
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class AirRaycast : Graphic
{
    protected AirRaycast()
    {
        useLegacyMeshGeneration = false;
        raycastTarget = true;
    }

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        toFill.Clear();
    }
}
