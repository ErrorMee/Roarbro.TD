using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 资源审核
/// </summary>
public class AssetAuditing : AssetPostprocessor
{
    public const string ToggleAutoRefreshGroup = "ToggleAutoRefreshGroup";

    static readonly HashSet<string> recordPaths = new();
    /// <summary>
    /// 在完成任意数量的资源导入后（当资源进度条到达末尾时）调用此函数
    /// </summary>
    /// <param name="importedAssets"></param>
    /// <param name="deletedAssets"></param>
    /// <param name="movedAssets"></param>
    /// <param name="movedFromAssetPaths"></param>
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        bool autoRefreshGroup = EditorPrefs.GetBool(ToggleAutoRefreshGroup);

        if (!autoRefreshGroup)
        {
            return;
        }

        bool findArt = true;
        if (!FindArt(importedAssets))
        {
            if (!FindArt(deletedAssets))
            {
                if (!FindArt(movedAssets))
                {
                    if (!FindArt(movedFromAssetPaths))
                    {
                        findArt = false;
                    }
                }
            }
        }

        if (findArt)
        {
            ManagedAddressable.FastRefreshGroup();
        }
    }

    static bool FindArt(string[] assets)
    {
        for (int i = 0; i < assets.Length; i++)
        {
            string path = assets[i];
            if (path.IndexOf("Assets/Art") == 0)
            {
                if (recordPaths.Contains(path))
                {
                    continue;
                }
                recordPaths.Add(path);
                return true;
            }
        }
        return false;
    }
}
