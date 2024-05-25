using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;

public static class OpenFolderUtil
{

    [@MenuItem(ProjectConfigs.company + "/OpenFolder/PersistentDataPath", false, 201)]
    public static void OpenPersistentDataPath()
    {
        var targetDir = new DirectoryInfo(Application.persistentDataPath);
        OpenDirectory(targetDir.FullName);
    }

    [@MenuItem(ProjectConfigs.company + "/OpenFolder/CacheForWritingPath", false, 202)]
    public static void OpenCacheForWritingPath()
    {
        var targetDir = new DirectoryInfo(Caching.currentCacheForWriting.path);
        OpenDirectory(targetDir.FullName);
    }

    [@MenuItem(ProjectConfigs.company + "/OpenFolder/ApplicationDataPath", false, 203)]
    public static void OpenApplicationDataPath()
    {
        var targetDir = new DirectoryInfo(Application.dataPath);
        OpenDirectory(targetDir.FullName);
    }

    [@MenuItem(ProjectConfigs.company + "/OpenFolder/StreamingAssetsPath", false, 204)]
    public static void OpenStreamingAssetsPath()
    {
        var targetDir = new DirectoryInfo(Application.streamingAssetsPath);
        OpenDirectory(targetDir.FullName);
    }

    [@MenuItem(ProjectConfigs.company + "/OpenFolder/AddressablesLocalBuildPath", false, 205)]
    public static void OpenAddressablesLocalBuildPath()
    {
        var targetDir = new DirectoryInfo("Library/com.unity.addressables/aa/Android/Android");
        OpenDirectory(targetDir.FullName);
    }

    [@MenuItem(ProjectConfigs.company + "/OpenFolder/LocalServerDataPath", false, 206)]
    public static void OpenLocalServerDataPath()
    {
        var targetDir = new DirectoryInfo("ServerData");
        OpenDirectory(targetDir.FullName);
    }

    [@MenuItem(ProjectConfigs.company + "/OpenFolder/AppPath", false, 207)]
    public static void OpenAppPath()
    {
        var targetDir = new DirectoryInfo(Application.dataPath + "/../App");
        OpenDirectory(targetDir.FullName);
    }

    [@MenuItem(ProjectConfigs.company + "/OpenFolder/ArtPath", false, 208)]
    public static void OpenArtPath()
    {
        var targetDir = new DirectoryInfo(Application.dataPath + "/../../Art");
        OpenDirectory(targetDir.FullName);
    }

    static string shellPath;
    public static void OpenDirectory(string path)
    {
        if (string.IsNullOrEmpty(path)) return;

        if (!Directory.Exists(path))
        {
            UnityEngine.Debug.LogError("No Directory: " + path);
            return;
        }

        //Application.dataPath 只能在主线程中获取
        int lastIndex = Application.dataPath.LastIndexOf("/");
        shellPath = Application.dataPath.Substring(0, lastIndex) + "/Shell/";

        // 新开线程防止锁死
        Thread newThread = new Thread(new ParameterizedThreadStart(CmdOpenDirectory));
        newThread.Start(path);
    }

    static public string GetRelativeAssetsPath(string path)
    {
        return "Assets" + Path.GetFullPath(path).Replace(Path.GetFullPath(Application.dataPath), "").Replace('\\', '/');
    }

    private static void CmdOpenDirectory(object obj)
    {
        Process p = new();
#if UNITY_EDITOR_WIN
        p.StartInfo.FileName = "cmd.exe";
        p.StartInfo.Arguments = "/c start " + obj.ToString();
#elif UNITY_EDITOR_OSX
        p.StartInfo.FileName = "bash";
        string shPath = shellPath + "openDir.sh";
        p.StartInfo.Arguments = shPath + " " + obj.ToString();
#endif
        //UnityEngine.Debug.Log(p.StartInfo.Arguments);
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.CreateNoWindow = true;
        p.Start();

        p.WaitForExit();
        p.Close();
    }
}
