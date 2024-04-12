using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StaticBlurFeature : RenderFeature<StaticBlurFeature, StaticBlurPass, StaticBlurSettings>
{
    private Camera camara = null;
    public static RenderTexture staticBlurRT;

    public override void Create()
    {
        if (!Application.isPlaying) { return; }

        base.Create();

        if (staticBlurRT == null)
        {
            staticBlurRT = RenderTexture.GetTemporary(
                (int)(Screen.width * pass.settings.rtScale), (int)(Screen.height * pass.settings.rtScale),
                0, RenderTextureFormat.RGB111110Float, RenderTextureReadWrite.Default);
            staticBlurRT.useMipMap = false;
            staticBlurRT.anisoLevel = 0;
        }
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (!Application.isPlaying) { return; }

        if (camara == null)
        {
            if (renderingData.cameraData.camera.name == pass.settings.camera)
            {
                camara = renderingData.cameraData.camera;
            }
        }

        if (renderingData.cameraData.camera == camara)
        {
            base.AddRenderPasses(renderer, ref renderingData);
        }
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        pass.Setup(renderer.cameraColorTargetHandle);
    }
}