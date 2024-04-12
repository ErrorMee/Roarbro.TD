using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[Serializable]
public class RenderFeatureSettings
{
    public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    public Material material;
}
