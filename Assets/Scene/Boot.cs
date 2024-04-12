using UnityEngine;
using UnityEngine.Rendering;

public class Boot: MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void BootFunction()
    {
        GraphicsSettings.useScriptableRenderPipelineBatching = true;

        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClassUtil.Init();
        }

        Angle720Util.Init();
        OptStrUtil.Init();
        EnumUtil.Init();

        Debug.Log("OnApplicationBoot");

    }

    [SerializeField] BootConfig bootConfig;
    public static BootConfig Config;

    [SerializeField] GameObject analyzer;

    private void Awake()
    {
        Config = bootConfig;
        Application.targetFrameRate = bootConfig.fps;

#if UNITY_EDITOR
        Destroy(analyzer);
#else
        int iconIndex = ArchiveModel.GetInt(ArchiveEnum.IconIndex, 0, false);
        if (bootConfig.analyzer && iconIndex > 0)
        {
            analyzer.SetActive(true);
        }
        else
        {
            Destroy(analyzer);
        }
#endif
    }

    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
    }
}
