#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class BootConfig : ScriptableObject
{
    public const string company = "♦Roarbro";

    public int fps;
    public bool analyzer;
    public bool netLog;
    public string HttpHost;
    public string LoginURL;
}

#if UNITY_EDITOR

[CustomEditor(typeof(BootConfig))]
public class BootConfigEditor : Editor
{
    BootConfig Config => target as BootConfig;

    readonly int[] fpsInts = new int[] { 30, 45, 60, 120 };
    readonly string[] fpsStrs = new string[] { "30", "45", "60", "120" };

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();

        if (GUILayout.Button("Save", GUILayout.Width(40)))
        {
            EditorUtility.SetDirty(Config);
            AssetDatabase.SaveAssetIfDirty(Config);
        }

        EditorGUILayout.LabelField(BootConfig.company);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("fps", GUILayout.Width(110)); Config.fps = EditorGUILayout.IntPopup(Config.fps, fpsStrs, fpsInts, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("analyzer", GUILayout.Width(110)); Config.analyzer = EditorGUILayout.Toggle(Config.analyzer);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("netLog", GUILayout.Width(110)); Config.netLog = EditorGUILayout.Toggle(Config.netLog);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("HttpHost      https://", GUILayout.Width(110)); Config.HttpHost = EditorGUILayout.TextField(Config.HttpHost, GUILayout.Width(200));
        EditorGUILayout.EndHorizontal();
        Config.LoginURL = "https://" + Config.HttpHost + "/login.php";
        EditorGUILayout.LabelField("LoginURL    " + Config.LoginURL, GUILayout.Width(350));

        EditorGUILayout.EndVertical();
    }
}
#endif
