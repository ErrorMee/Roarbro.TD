using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QualityConfigs))]
public class QualityConfigsEditor : ConfigsEditor<QualityConfig, QualityConfigs>
{
    [MenuItem(ConfigMenu + "Quality")]
    static void Create()
    {
        CreateConfigs<QualityConfigs>("Quality");
    }

    protected override void DrawHead()
    {
        languageRange = new Vector2Int(10000, 10006);
        base.DrawHead();
        EditorGUILayout.LabelField("quality", GUILayout.Width(50));
        EditorGUILayout.LabelField("color1", GUILayout.Width(40));
        EditorGUILayout.LabelField("color2", GUILayout.Width(40));
    }

    protected override void DrawConfig(int index, QualityConfig config)
    {
        GUI.color = config.color1;
        base.DrawConfig(index, config);
        config.quality = (QualityEnum)EditorGUILayout.EnumPopup((QualityEnum)config.id, GUILayout.Width(50));

        config.id = (int)config.quality;
        config.color1 = EditorGUILayout.ColorField(config.color1, GUILayout.Width(40));
        config.color2 = EditorGUILayout.ColorField(config.color2, GUILayout.Width(40));
    }
}