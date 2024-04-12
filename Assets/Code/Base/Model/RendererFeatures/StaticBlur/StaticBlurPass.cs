using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class StaticBlurPass : RenderPass<StaticBlurSettings>
{
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get(profileTag);

        cmd.Blit(colorSource, StaticBlurFeature.staticBlurRT, settings.material, 0);
        context.ExecuteCommandBuffer(cmd);
        
        StaticBlurFeature.Instance.SetActive(false);

        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }
}