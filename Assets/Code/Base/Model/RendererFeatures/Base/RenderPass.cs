using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RenderPass<Settings> : ScriptableRenderPass where Settings : RenderFeatureSettings
{
    public Settings settings;

    protected string profileTag = string.Empty;
    protected RTHandle colorSource;

    public void Setup(RTHandle colorSource)
    {
        this.colorSource = colorSource;
        profileTag = GetType().ToString();
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        base.Configure(cmd, cameraTextureDescriptor);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        throw new NotImplementedException();
    }

    public override void FrameCleanup(CommandBuffer cmd)
    {
        base.FrameCleanup(cmd);
    }
}