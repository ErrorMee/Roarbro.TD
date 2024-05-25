using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyConfigs))]
public class EnemyConfigsEditor : ConfigsEditor<EnemyConfig, EnemyConfigs>
{
    [MenuItem(ConfigMenu + "Enemy")]
    static void Create()
    {
        CreateConfigs<EnemyConfigs>("Enemy");
    }

    protected override void DrawHead()
    {
        base.DrawHead();
        EditorGUILayout.LabelField("avatar", GUILayout.Width(40));
        EditorGUILayout.LabelField("color", GUILayout.Width(40));
        EditorGUILayout.LabelField("speed", GUILayout.Width(40));
        EditorGUILayout.LabelField("hp:basic-percent", GUILayout.Width(100));
    }

    protected override void DrawMenuAdd()
    {
        base.DrawMenuAdd();
        //TODO
    }

    protected override void DrawConfig(int index, EnemyConfig config)
    {
        base.DrawConfig(index, config);

        config.avatar = EditorGUILayout.IntField(config.avatar, GUILayout.Width(40));
        config.color = EditorGUILayout.ColorField(config.color, GUILayout.Width(40));
        config.speed = EditorGUILayout.FloatField(config.speed, GUILayout.Width(40));



        config.hp.basic = EditorGUILayout.FloatField(config.hp.basic, GUILayout.Width(48));
        config.hp.percent = EditorGUILayout.FloatField(config.hp.percent, GUILayout.Width(48));
    }
}