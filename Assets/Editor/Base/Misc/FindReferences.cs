using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class FindReferences
{
    [MenuItem("Assets/" + ProjectConfigs.company + "/References/Shader", true)]
    static bool CheckShaderReferences()
    {
        if (Selection.activeObject)
        {
            string activeType = Selection.activeObject.GetType().ToString();
            if (activeType.Contains("Shader"))
            {
                return true;
            }
        }
        return false;
    }

    [MenuItem("Assets/" + ProjectConfigs.company + "/References/Shader", false)]
    static void ShaderReferences()
    {
        References(Selection.activeObject, "*.mat");
    }

    [MenuItem("Assets/" + ProjectConfigs.company + "/References/Material", true)]
    static bool CheckMaterialReferences()
    {
        if (Selection.activeObject)
        {
            string activeType = Selection.activeObject.GetType().ToString();
            if (activeType.Contains("Material"))
            {
                return true;
            }
        }
        return false;
    }

    [MenuItem("Assets/" + ProjectConfigs.company + "/References/Material", false)]
    static void MaterialReferences()
    {
        References(Selection.activeObject, "*.prefab");
    }

    [MenuItem("Assets/" + ProjectConfigs.company + "/References/Prefab", true)]
    static bool CheckPrefabReferences()
    {
        if (Selection.activeObject)
        {
            string activeType = Selection.activeObject.GetType().ToString();
            if (activeType.Contains("GameObject"))
            {
                return true;
            }
        }
        return false;
    }

    [MenuItem("Assets/" + ProjectConfigs.company + "/References/Prefab", false)]
    static void PrefabReferences()
    {
        References(Selection.activeObject, "*.prefab");
    }

    static public List<object> References(UnityEngine.Object target, string searchPattern)
    {
        string selectGuid = string.Empty;

        if (target)
        {
            string path = AssetDatabase.GetAssetPath(target);
            if (!string.IsNullOrEmpty(path))
            {
                selectGuid = AssetDatabase.AssetPathToGUID(path);
                Debug.Log("Select:" + target.name + " guid:" + selectGuid + " path:" + path);
            }
        }

        List<object> references = new();

        if (selectGuid != string.Empty)
        {
            string rootPath = Application.dataPath + "/Art";

            string[] files = Directory.GetFiles(rootPath, searchPattern, SearchOption.AllDirectories);

            int findCount = 0;
            for (int i = 0; i < files.Length; i++)
            {
                string file = OpenFolderUtil.GetRelativeAssetsPath(files[i]);
                string[] deps = AssetDatabase.GetDependencies(file);

                for (int j = 0; j < deps.Length; j++)
                {
                    string depFile = deps[j];
                    string guid = AssetDatabase.AssetPathToGUID(depFile);

                    if (guid == selectGuid)
                    {
                        references.Add(AssetDatabase.LoadAllAssetsAtPath(file)[0]);
                        Debug.Log(string.Format("{0} Find:{1}", ++findCount, file));
                    }
                }
            }
            Debug.Log("AllFind:" + findCount);
        }
        return references;
    }
}