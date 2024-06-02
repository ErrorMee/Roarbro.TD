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
        EditorGUILayout.LabelField("speed", GUILayout.Width(40));

        if (BallConfigs.Instance == null)
        {
            BallConfigs.Instance = GetConfigs("Ball") as BallConfigs;
        }
    }

    protected override void DrawMenuAdd()
    {
        base.DrawMenuAdd();
        GotoConfigs("Ball", 45);
    }

    protected override void DrawConfig(int index, BulletConfig config)
    {
        base.DrawConfig(index, config);
        BallConfig ballConfig = BallConfigs.Instance.GetConfigByID(config.id);
        GUI.color = ballConfig.color;
        config.size = EditorGUILayout.FloatField(config.size, GUILayout.Width(40));
        config.speed = EditorGUILayout.FloatField(config.speed, GUILayout.Width(40));
    }
}