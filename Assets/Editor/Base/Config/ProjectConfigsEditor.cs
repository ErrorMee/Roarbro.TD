using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProjectConfigs))]
public class ProjectConfigsEditor : ConfigsEditor<ProjectConfig, ProjectConfigs>
{
    [MenuItem(ConfigMenu + "Project")]
    static void Create()
    {
        CreateConfigs<ProjectConfigs>("Project");
    }

    readonly int[] fpsInts = new int[] { 30, 45, 60, 120 };
    readonly string[] fpsStrs = new string[] { "30", "45", "60", "120" };

    protected override void DrawHead()
    {
        base.DrawHead();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        Configs.HttpHost = PreferredGUIUtil.DrawPreferredTextArea("LoginURL      https://", Configs.HttpHost, 200);
        DrawPreferredLabel("/login.php");
        Configs.LoginURL = "https://" + Configs.HttpHost + "/login.php";
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("fps", GUILayout.Width(110)); Configs.fps = EditorGUILayout.IntPopup(Configs.fps, fpsStrs, fpsInts, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("analyzer", GUILayout.Width(110)); Configs.analyzer = EditorGUILayout.Toggle(Configs.analyzer);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("netLog", GUILayout.Width(110)); Configs.netLog = EditorGUILayout.Toggle(Configs.netLog);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    protected override void DrawMenuAdd()
    {
        base.DrawMenuAdd();
        //TODO
    }

    protected override void DrawConfig(int index, ProjectConfig config)
    {
        base.DrawConfig(index, config);
        //TODO
    }
}