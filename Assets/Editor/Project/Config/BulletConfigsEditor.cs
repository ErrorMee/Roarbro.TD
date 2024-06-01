using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BulletConfigs))]
public class BulletConfigsEditor : ConfigsEditor<BulletConfig, BulletConfigs>
{
    [MenuItem(ConfigMenu + "Bullet")]
    static void Create()
    {
        CreateConfigs<BulletConfigs>("Bullet");
    }

    protected override void DrawHead()
    {
        base.DrawHead();
        EditorGUILayout.LabelField("size", GUILayout.Width(40));
        EditorGUILayout.LabelField("color", GUILayout.Width(40));
        EditorGUILayout.LabelField("speed", GUILayout.Width(40));
    }

    protected override void DrawMenuAdd()
    {
        base.DrawMenuAdd();
        //TODO
    }

    protected override void DrawConfig(int index, BulletConfig config)
    {
        base.DrawConfig(index, config);
        config.size = EditorGUILayout.FloatField(config.size, GUILayout.Width(40));
        config.color = EditorGUILayout.ColorField(config.color, GUILayout.Width(40));
        config.speed = EditorGUILayout.FloatField(config.speed, GUILayout.Width(40));
    }
}