using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LanguageConfigs))]
public class LanguageConfigsEditor : ConfigsEditor<LanguageConfig, LanguageConfigs>
{
    [MenuItem(ConfigMenu + "Language")]
    static void Create()
    {
        CreateConfigs<LanguageConfigs>("Language");
    }

    protected override void DrawHead()
    {
        base.DrawHead();
        GUI.color = Color.white;
        DrawVertical(512, () =>
        {
            DrawHorizontal(512, () =>
            {
                DrawLabel("Language types:", 100);
                if (GUILayout.Button("+", GUILayout.Width(20)))
                {
                    LanguageType[] allNew = new LanguageType[Configs.types.Length + 1];
                    Configs.types.CopyTo(allNew, 0);
                    Configs.types = allNew;
                }
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    LanguageType[] allNew = new LanguageType[Configs.types.Length - 1];
                    Configs.types.CopyTo(allNew, 0);
                    Configs.types = allNew;
                }
            });

            for (int i = 0; i < Configs.types.Length; i++)
            {
                DrawHorizontal(200, () =>
                {
                    Configs.types[i].type = (SystemLanguage)EditorGUILayout.EnumPopup(Configs.types[i].type, GUILayout.Width(140));
                    
                    Configs.types[i].code = PreferredGUIUtil.DrawPreferredTextField(Configs.types[i].code);
                    Configs.types[i].tag = PreferredGUIUtil.DrawPreferredTextField(Configs.types[i].tag);
                });
            }
        });
    }

    protected override void DrawConfig(int index, LanguageConfig config)
    {
        base.DrawConfig(index, config);

        if (config.values.Length < Configs.types.Length)
        {
            string[] allNew = new string[Configs.types.Length];
            config.values.CopyTo(allNew, 0);
            config.values = allNew;
        }

        int widthTest = (int)GUI.skin.label.CalcSize(new GUIContent(config.values[0] + Configs.types[0].code)).x + 8;
        if (widthTest > 80)
        {
            for (int i = 0; i < Configs.types.Length; i += 2)
            {
                EditorGUILayout.BeginVertical(GUILayout.Width(widthTest));
                EditorGUILayout.BeginHorizontal(GUILayout.Width(widthTest));
                config.values[i] = PreferredGUIUtil.DrawPreferredTextArea(Configs.types[i].code, config.values[i], 256);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal(GUILayout.Width(widthTest));
                if (config.values.Length >= i)
                {
                    config.values[i + 1] = PreferredGUIUtil.DrawPreferredTextArea(Configs.types[i + 1].code, config.values[i + 1], 256);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
        }
        else if (widthTest > 200)
        {
            for (int i = 0; i < Configs.types.Length; i++)
            {
                EditorGUILayout.BeginVertical(GUILayout.Width(256));
                EditorGUILayout.BeginHorizontal(GUILayout.Width(256));
                config.values[i] = PreferredGUIUtil.DrawPreferredTextArea(Configs.types[i].code, config.values[i], 256);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
        }
        else
        {
            for (int i = 0; i < Configs.types.Length; i++)
            {
                config.values[i] = PreferredGUIUtil.DrawPreferredTextArea(Configs.types[i].code, config.values[i], 128);
            }
        }

    }
}