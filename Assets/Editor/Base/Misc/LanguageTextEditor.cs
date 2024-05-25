
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LanguageText), true), CanEditMultipleObjects]
public class LanguageTextEditor : TMP_EditorPanelUI
{
    [MenuItem("CONTEXT/Graphic/Convert To LanguageText", true)]
    static bool CheckConvertToLanguageText(MenuCommand command)
    {
        return command.context.GetType() == typeof(TextMeshProUGUI);
    }

    [MenuItem("CONTEXT/Graphic/Convert To TextMeshProUGUI", true)]
    static bool CheckConvertToTextMeshProUGUI(MenuCommand command)
    {
        return ComponentConverter.CanConvertTo<TextMeshProUGUI>(command.context);
    }

    [MenuItem("CONTEXT/Graphic/Convert To TextMeshProUGUI", false)]
    static void ConvertToTextMeshProUGUI(MenuCommand command)
    {
        ComponentConverter.ConvertTo<TextMeshProUGUI>(command.context);
    }

    [MenuItem("CONTEXT/Graphic/Convert To LanguageText", false)]
    static void ConvertToLanguageText(MenuCommand command)
    {
        TextMeshProUGUI textMeshProUGUI = command.context as TextMeshProUGUI;

        var sourceProps = typeof(TextMeshProUGUI).GetProperties();
        List<object> sourcePropsValue = new();
        foreach (var prop in sourceProps)
        {
            if (prop.CanWrite)
            {
                if (!prop.Name.Contains("Material"))
                {
                    sourcePropsValue.Add(prop.GetValue(textMeshProUGUI, null));
                }
                else
                {
                    sourcePropsValue.Add(null);
                }
            }
            else
            {
                sourcePropsValue.Add(null);
            }
        }

        GameObject gameObject = textMeshProUGUI.gameObject;
        DestroyImmediate(textMeshProUGUI);

        LanguageText languageText = gameObject.AddComponent<LanguageText>();

        var targetProps = typeof(LanguageText).GetProperties();
        int index = 0;
        foreach (var prop in targetProps)
        {
            object sourcePropValue = sourcePropsValue[index++];
            if (prop.CanWrite)
            {
                if (sourcePropValue != null)
                {
                    prop.SetValue(languageText, sourcePropValue, null);
                }
            }
        }
    }

    LanguageText Target => target as LanguageText;

    LanguageConfigs languageConfigs;

    int[] languageIDs;
    string[] languageStrs;


    void DrawLanguageSelect()
    {
        if (languageConfigs == null)
        {
            languageConfigs = AssetDatabase.LoadAssetAtPath<LanguageConfigs>("Assets/Art/Config/LanguageConfigs.asset");

            List<LanguageConfig> rangeLanguageConfigs = new();
            for (int i = 0; i < languageConfigs.all.Length; i++)
            {
                LanguageConfig config = languageConfigs.all[i];
                if (string.IsNullOrEmpty(config.values[0]) == false)
                {
                    rangeLanguageConfigs.Add(config);
                }
            }

            languageIDs = new int[rangeLanguageConfigs.Count];
            languageStrs = new string[rangeLanguageConfigs.Count];
            for (int i = 0; i < rangeLanguageConfigs.Count; i++)
            {
                LanguageConfig config = rangeLanguageConfigs[i];
                languageIDs[i] = config.id;
                languageStrs[i] = config.id.ToString() + " " + config.values[0];
            }
        }

        EditorGUILayout.BeginHorizontal(GUILayout.Width(300));

        if (GUILayout.Button("Save", GUILayout.Width(40)))
        {
            EditorUtility.SetDirty(Target);
            AssetDatabase.SaveAssetIfDirty(Target);

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }

        GUILayout.Label("LanguageID", GUILayout.Width(75));
        Target.languageID = EditorGUILayout.IntPopup(Target.languageID, languageStrs, languageIDs);
        LanguageConfig languageConfig = languageConfigs.GetConfigByID(Target.languageID);
        if (languageConfig != null)
        {
            Target.text = languageConfig.values[0];
        }

        EditorGUILayout.EndHorizontal();
    }

    public override void OnInspectorGUI()
    {
        // Make sure Multi selection only includes TMP Text objects.
        if (IsMixSelectionTypes()) return;

        serializedObject.Update();

        DrawLanguageSelect();
        //DrawTextInput();

        DrawMainSettings();

        DrawExtraSettings();

        EditorGUILayout.Space();

        if (serializedObject.ApplyModifiedProperties() || m_HavePropertiesChanged)
        {
            m_TextComponent.havePropertiesChanged = true;
            m_HavePropertiesChanged = false;
            EditorUtility.SetDirty(target);
        }
    }
}