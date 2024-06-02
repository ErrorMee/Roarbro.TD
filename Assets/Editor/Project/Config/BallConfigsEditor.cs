using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BallConfigs))]
public class BallConfigsEditor : ConfigsEditor<BallConfig, BallConfigs>
{
    [MenuItem(ConfigMenu + "Ball")]
    static void Create()
    {
        CreateConfigs<BallConfigs>("Ball");
    }

    protected override void DrawHead()
    {
        base.DrawHead();
        EditorGUILayout.LabelField("color", GUILayout.Width(40));
        EditorGUILayout.LabelField("attack", GUILayout.Width(40));
        EditorGUILayout.LabelField("attCD", GUILayout.Width(40));
        EditorGUILayout.LabelField("radius", GUILayout.Width(40));
    }

    protected override void DrawMenuAdd()
    {
        base.DrawMenuAdd();
        GotoConfigs("Bullet", 45);
    }

    protected override void DrawConfig(int index, BallConfig config)
    {
        base.DrawConfig(index, config);
        GUI.color = config.color;
        config.color = EditorGUILayout.ColorField(config.color, GUILayout.Width(40));
        config.attack = EditorGUILayout.IntField(config.attack, GUILayout.Width(40));
        config.attCD = EditorGUILayout.FloatField(config.attCD, GUILayout.Width(40));
        config.attRadius = EditorGUILayout.FloatField(config.attRadius, GUILayout.Width(40));
    }
}