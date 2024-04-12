using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ��Դ���
/// </summary>
public class AssetAuditing : AssetPostprocessor
{
    public const string ToggleAutoRefreshGroup = "ToggleAutoRefreshGroup";

    static readonly HashSet<string> recordPaths = new();
    /// <summary>
    /// �����������������Դ����󣨵���Դ����������ĩβʱ�����ô˺���
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
