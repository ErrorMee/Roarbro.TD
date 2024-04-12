using System.Diagnostics;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityToolbarExtender;

[InitializeOnLoad]
public class RoarbroToolbar
{
    static RoarbroToolbar()
    {
        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
    }

    static void OnToolbarGUI()
    {
        GUILayout.FlexibleSpace();

        if (GUILayout.Button(new GUIContent("BuildAnalyze", "Build App Rlease Analyze")))
        {
            BuildApp.BuildAnalyze();
        }

        if (GUILayout.Button(new GUIContent("CreateWindow", "Create New Window")))
        {
            CreateWindow.OpenCreateWindow();
        }

        if (GUILayout.Button(new GUIContent("Guide", "Open ProjectGuide With VSCODE")))
        {
            //string codeExePath = "D:/Users/Xpeng/AppData/Local/Programs/Microsoft VS Code/Code.exe";
            //Process p = Process.Start(codeExePath, Application.dataPath + "/Doc/ProjectGuide.md");
            Process.Start("code", Application.dataPath + "/Doc/ProjectGuide.md");
        }

        if (GUILayout.Button(new GUIContent("RefreshAsset", "Fast Refresh Addressable Group")))
        {
            ManagedAddressable.FastRefreshGroup();
        }

        if (GUILayout.Button(new GUIContent("OpenBoot", "Open Boot Scene")))
        {
            EditorSceneManager.OpenScene("Assets/Scene/Boot.unity", OpenSceneMode.Single);
        }
    }
}