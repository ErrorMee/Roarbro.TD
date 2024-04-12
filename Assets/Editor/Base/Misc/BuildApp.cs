using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildApp : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }
    public void OnPreprocessBuild(BuildReport report)
    {
        Debug.Log("BuildPrepare for target " + report.summary.platform + " at path " + report.summary.outputPath);
    }

    [MenuItem(BootConfig.company + "/BuildApp/Debug", false, 1)]
    public static void BuildDebug()
    {
        OnBuild(true, true);
        OpenFolderUtil.OpenAppPath();
    }

    [MenuItem(BootConfig.company + "/BuildApp/Rlease", false, 2)]
    public static void BuildRlease()
    {
        OnBuild(false, false);
        OpenFolderUtil.OpenAppPath();
    }

    [MenuItem(BootConfig.company + "/BuildApp/Analyze", false, 3)]
    public static void BuildAnalyze()
    {
        OnBuild(false, true);
        OpenFolderUtil.OpenAppPath();
    }

    static void OnBuild(bool debug, bool analyze)
    {
        ManagedAddressable.ClearRefreshGroup();
        ManagedAddressable.BuildBundle();

        string appDir = Application.dataPath + "/../App";
        if (Directory.Exists(appDir))
        {
            int keepFilesCount = 3; // 保留的文件数量

            DirectoryInfo directory = new DirectoryInfo(appDir);
            FileInfo[] files = directory.GetFiles("*.apk")
                                        .OrderByDescending(f => f.CreationTime)
                                        .Skip(keepFilesCount - 1)
                                        .ToArray();
            DirectoryInfo[] dirs = directory.GetDirectories();

            foreach (FileInfo file in files)
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in dirs)
            {
                dir.Delete(true);
            }
        }
        else
        {
            Directory.CreateDirectory(appDir);
        }
        
        string strName = "App/" + Application.productName + "-{0}-{1}.apk";

        string debugAnalyze = debug ? "D" : analyze ? "A" : "R";

        string target_file_name = string.Format(strName, DateTime.Now.ToString("yyMMdd-HHmmss")[1..], debugAnalyze);
        BuildOptions buildOptions = BuildOptions.CompressWithLz4;
        if (debug)
        {
            buildOptions |= BuildOptions.Development;
        }

        BootConfig bootConfig = AssetDatabase.LoadAssetAtPath<BootConfig>("Assets/Scene/BootConfig.asset");
        bootConfig.analyzer = analyze;
        EditorUtility.SetDirty(bootConfig);
        AssetDatabase.SaveAssetIfDirty(bootConfig);

        BuildPipeline.BuildPlayer(GetEnabledScenes(), target_file_name, BuildTarget.Android, buildOptions);
    }

    static string[] GetEnabledScenes()
    {
        return (
            from scene in EditorBuildSettings.scenes
            where scene.path.Contains("Scene/")
            select scene.path
        ).ToArray();
    }
}