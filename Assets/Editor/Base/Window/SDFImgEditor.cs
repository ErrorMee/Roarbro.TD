
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(SDFImg), true)]
[CanEditMultipleObjects]
public class SDFImgEditor : GraphicEditor
{
    SDFImg Target;
    SerializedProperty slice, sliceRadius, id, outline, onion;


    protected override void OnEnable()
    {
        base.OnEnable();
        Target = target as SDFImg;
        slice = serializedObject.FindProperty("slice");
        sliceRadius = serializedObject.FindProperty("sliceRadius");
        id = serializedObject.FindProperty("id");
        outline = serializedObject.FindProperty("outline");
        onion = serializedObject.FindProperty("onion");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        AppearanceControlsGUI();
        RaycastControlsGUI();
        MaskableControlsGUI();

        EditorGUILayout.BeginHorizontal();
        PreferredGUIUtil.DrawPreferredLabel("Size");
        ScaleSize(2); ScaleSize(0.5f); ChangeSize(32); ChangeSize(-32); ChangeSize(8); ChangeSize(-8); PreferredGUIUtil.DrawPreferredLabel("Width");
        ChangeSize(8, true); ChangeSize(-8, true); PreferredGUIUtil.DrawPreferredLabel("Height"); ChangeSize(8, false); ChangeSize(-8, false);
        EditorGUILayout.EndHorizontal();

        Target.Slice = (SDFImg.SliceMode)EditorGUILayout.EnumPopup("Slice", Target.Slice);
        slice.intValue = (int)Target.Slice;

        if (Target.Slice == SDFImg.SliceMode.Nine || Target.Slice == SDFImg.SliceMode.Eight)
        {
            float minEdge = Mathf.Min(Target.rectTransform.rect.size.x, Target.rectTransform.rect.size.y);
            int maxRadius = Mathf.CeilToInt(minEdge * 0.5f) - 1;
            sliceRadius.intValue = EditorGUILayout.IntSlider("        radius [ 1, " + maxRadius + " ] ", sliceRadius.intValue, 1, maxRadius);
        }

        EditorGUILayout.LabelField("Effects");
        DrawSliderProperty(id, SDFImg.ID_MAX);
        DrawSliderProperty(outline, SDFImg.EFFECT_MAX);
        DrawSliderProperty(onion, SDFImg.EFFECT_MAX);

        EditorGUI.EndChangeCheck();
        serializedObject.ApplyModifiedProperties();
    }

    private void ScaleSize(float scaler)
    {
        Vector2 size = Target.rectTransform.sizeDelta;
        string label = "*" + scaler;
        if (GUILayout.Button(label, PreferredGUIUtil.GetButtonWidth(label)))
        {
            size *= scaler;
            Target.rectTransform.sizeDelta = size;
        }
    }

    private void ChangeSize(int change)
    {
        Vector2 size = Target.rectTransform.sizeDelta;
        string label = (change > 0 ? "+" : "") + change;
        if (GUILayout.Button(label, PreferredGUIUtil.GetButtonWidth(label)))
        {
            size.x += change; size.y += change;
            Target.rectTransform.sizeDelta = size;
        }
    }

    private void ChangeSize(int change, bool isWidth)
    {
        Vector2 size = Target.rectTransform.sizeDelta;
        string label = (change > 0 ? "+" : "") + change;
        if (GUILayout.Button(label, PreferredGUIUtil.GetButtonWidth(label)))
        {
            if (isWidth)
            {
                size.x += change;
            }
            else
            {
                size.y += change;
            }
            Target.rectTransform.sizeDelta = size;
        }
    }

    private void DrawSliderProperty(SerializedProperty property, int max)
    {
        property.intValue = EditorGUILayout.IntSlider("    " + property.name, property.intValue, 0, max - 1);
    }

    //#region MenuItem
    //[MenuItem(WidgetEditor.rootMenu + "SDFImg", false, 1)]
    //public static void CreateImage(MenuCommand menuCommand)
    //{
    //    UIMenuUtil.CreateUIElementRoot<SDFImg>(menuCommand);
    //}
    //#endregion
}
