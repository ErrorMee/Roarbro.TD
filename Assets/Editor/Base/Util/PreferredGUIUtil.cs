#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class PreferredGUIUtil
{
    static readonly GUIStyle middleCenterStyle = new GUIStyle(GUI.skin.label)
    {
        alignment = TextAnchor.MiddleCenter
    };

    public static GUILayoutOption GetButtonWidth(string label)
    {
        return GetLabalWidth(label + 4);
    }

    public static GUILayoutOption GetLabalWidth(string label, int blank = 0)
    {
        return GUILayout.Width(Mathf.CeilToInt(GUI.skin.label.CalcSize(new GUIContent(label)).x + blank));
    }

    public static void DrawPreferredLabel(string label, int blank = 0)
    {
        EditorGUILayout.LabelField(label, middleCenterStyle, GetLabalWidth(label, blank));
    }

    public static int DrawPreferredInt(int value, int blank = 0)
    {
        return EditorGUILayout.IntField(value, GetLabalWidth(value.ToString(), blank));
    }

    public static string DrawPreferredTextField(string text)
    {
        int width = (int)GUI.skin.label.CalcSize(new GUIContent(text)).x + 2;
        width = Mathf.Clamp(width, 10, 120);
        return EditorGUILayout.TextField(text, GUILayout.Width(width));
    }

    public static string DrawPreferredTextArea(string lable, string text, int maxWidth = 120)
    {
        DrawPreferredLabel(lable);

        int width = (int)GUI.skin.label.CalcSize(new GUIContent(text)).x + 2;
        width = Mathf.Clamp(width, 10, maxWidth);
        return EditorGUILayout.TextArea(text, GUILayout.Width(width));
    }
}
#endif
