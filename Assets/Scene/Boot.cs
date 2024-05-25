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

    [SerializeField] GameObject analyzer;

    private void Awake()
    {
        Application.targetFrameRate = 30;

        ConfigModel.configLoadCompleted += OnConfigLoadCompleted;
        ConfigModel.Instance.Init();
        TimerModel.Init();
    }

    void OnConfigLoadCompleted()
    {
        ConfigModel.configLoadCompleted -= OnConfigLoadCompleted;
        Application.targetFrameRate = ProjectConfigs.Instance.fps;

#if UNITY_EDITOR
        Destroy(analyzer);
#else
        int iconIndex = ArchiveModel.GetInt(ArchiveEnum.IconIndex, 0, false);
        if (ProjectConfigs.Instance.analyzer && iconIndex > 0)
        {
            analyzer.SetActive(true);
        }
        else
        {
            Destroy(analyzer);
        }
#endif
        LanguageModel.Instance.Init();
        WindowModel.Instance.Init();
    }

    void Update()
    {
        AddressModel.Instance.Update();
        HttpModel.Instance.Update();
    }

    private void FixedUpdate()
    {
        TimerModel.Instance.FixedUpdate();
    }

    private void OnApplicationQuit()
    {
        ProjectConfigs.isQuiting = true;
        Debug.Log("OnApplicationQuit");
    }
}
