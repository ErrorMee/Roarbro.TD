using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RenderFeature<Feature, Pass, Settings> : ScriptableRendererFeature
    where Pass : RenderPass<Settings>, new() where Settings : RenderFeatureSettings, new()
{
    #region singleton
    protected static RenderFeature<Feature, Pass, Settings> instance;
    public static RenderFeature<Feature, Pass, Settings> Instance
    {
        get { return instance; }
    }
    #endregion

    protected Pass pass = new();
    [SerializeField]
    Settings settings = new();

    public override void Create()
    {
        pass.settings = settings;
        instance = this;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        pass.renderPassEvent = settings.renderPassEvent;
        
        renderer.EnqueuePass(pass);
    }

    public virtual void Show(Material material)
    {
        pass.settings.material = material;
        SetActive(true);
    }
}