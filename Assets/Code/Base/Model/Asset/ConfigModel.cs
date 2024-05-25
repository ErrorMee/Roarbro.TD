using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ConfigModel : Singleton<ConfigModel>, IDestroy
{
    private Dictionary<string, ConfigsBase> m_Configs = new Dictionary<string, ConfigsBase>();

    public static Action configLoadCompleted;

    public void Init() 
    {
        List<string> labels = new() { "Config" };
        AsyncOperationHandle<IList<ConfigsBase>> asyncOperationHandle = 
        Addressables.LoadAssetsAsync<ConfigsBase>(labels, OnLoadConfig, Addressables.MergeMode.Union);
        asyncOperationHandle.Completed += OnLoadConfigCompolete;
    }

    private void OnLoadConfigCompolete(AsyncOperationHandle<IList<ConfigsBase>> asyncOperationHandle)
    {
        asyncOperationHandle.Completed -= OnLoadConfigCompolete;
        Addressables.Release(asyncOperationHandle);//卸载配置
        configLoadCompleted?.Invoke();
    }

    private void OnLoadConfig(ConfigsBase configAsset)
    {
        Type configType = configAsset.GetType();
        string fullName = configType.FullName;
        m_Configs[fullName[0..^1] + configAsset.key] = configAsset;
    }

    public S GetConfigs<S, C>(string key = "") where S : ConfigsGenerics<C>
    {
        Type type = typeof(S);
        if (m_Configs.TryGetValue(type.FullName[0..^1] + key, out ConfigsBase configs))
        {
            return configs as S;
        }
        else
        {
            LogUtil.Log($">>>Not Find'Assets/Art/Config/{type.Name}.asset key={key}' Try [RefreshAsset]", Color.red);
            return null;
        }
    }
}
