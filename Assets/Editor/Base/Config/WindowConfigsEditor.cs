using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WindowConfigs))]
public class WindowConfigsEditor : ConfigsEditor<WindowConfig, WindowConfigs>
{
    [MenuItem(ConfigMenu + "Window")]
    static void Create()
    {
        CreateConfigs<WindowConfigs>("Window");
    }

    protected override void DrawHead()
    {
        base.DrawHead();
        EditorGUILayout.LabelField("Window", GUILayout.Width(100));
        EditorGUILayout.LabelField("Layer", GUILayout.Width(45));
        EditorGUILayout.LabelField("Show", GUILayout.Width(60));

        EditorGUILayout.LabelField("Depends", GUILayout.Width(130));
        EditorGUILayout.LabelField("bg", GUILayout.Width(60));
    }

    protected override void DrawConfig(int index, WindowConfig config)
    {
        if (config.layer == WindowLayerEnum.Bot)
        {
            GUI.color = Color.cyan;
        }
        else if (config.layer == WindowLayerEnum.Mid)
        {
            GUI.color = Color.white;
        }
        else if (config.layer == WindowLayerEnum.Top)
        {
            GUI.color = Color.yellow;
        }
        else
        {
            GUI.color = Color.black;
        }

        base.DrawConfig(index, config);

        config.id = (int)(WindowEnum)EditorGUILayout.EnumPopup((WindowEnum)config.id, GUILayout.Width(100));
        config.layer = (WindowLayerEnum)EditorGUILayout.EnumPopup(config.layer, GUILayout.Width(45));
        config.show = (WindowShowEnum)EditorGUILayout.EnumPopup(config.show, GUILayout.Width(60));

        EditorGUILayout.BeginVertical(GUILayout.Width(120));
        if (config.layer == WindowLayerEnum.Bot)
        {
            if (config.depends != null)
            {
                for (int i = 0; i < config.depends.Length; i++)
                {
                    DrawHorizontal(120, () =>
                    {
                        WindowEnum attrConfig = config.depends[i];
                        config.depends[i] = (WindowEnum)EditorGUILayout.EnumPopup(attrConfig, GUILayout.Width(100));
                        config.depends = ArrayDelete(config.depends, i);
                    });
                }
            }

            config.depends = ArrayOeprate(config.depends, config, false);
        }
        EditorGUILayout.EndVertical();

        if (config.layer == WindowLayerEnum.Bot)
        {
            config.bg = (BGEnum)EditorGUILayout.EnumPopup(config.bg, GUILayout.Width(60));
        }
    }
}