using System;
using UnityEngine;

[Serializable]
public class StaticBlurSettings : RenderFeatureSettings
{
    [Range(0.1f, 1.0f)]
    public float rtScale = 1.0f;
    public string camera = "Main Camera";
}