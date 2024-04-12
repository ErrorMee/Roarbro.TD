
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(SDFBtn), true)]
[CanEditMultipleObjects]
public class SDFBtnEditor : SelectableEditor
{

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();
    }
}
