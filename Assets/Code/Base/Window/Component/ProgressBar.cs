
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
#endif

public class ProgressBar : Image
{
    public Color[] barColors = new Color[] { new(1, 1, 1, 1), new(1, 1, 1, 1), new(1, 1, 1, 1), new(1, 1, 1, 1) };

    [Range(0, 1)] public float value = 0.5f;
    float valueTemp;
    [Range(0.001f, 0.02f)]
    public float valueGap = 0.01f;

    [SerializeField]
    private TextMeshProUGUI txt;

    [SerializeField]
    private bool solo = true;

    public void SetProgress(float crt, float max)
    {
        value = crt / max;
        if (txt)
        {
            txt.text = crt + "/" + max;
        }
        SetVerticesDirty();
    }

    private void Update()
    {
        if (valueTemp >= value)
        {
            if (valueTemp > value)
            {
                if (solo)
                {
                    valueTemp = 0;
                }
                else
                {
                    valueTemp -= valueGap;
                    if (valueTemp <= value)
                    {
                        valueTemp = value;
                    }
                    SetVerticesDirty();
                }
            }
            else
            {
                valueTemp = value;
            }
        }
        else
        {
            valueTemp += valueGap;
            if (valueTemp >= value)
            {
                valueTemp = value;
            }
            SetVerticesDirty();
        }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        float widthHalf = rectTransform.sizeDelta.x * 0.5f;
        float heigthHalf = rectTransform.sizeDelta.y * 0.5f;

        AddVert(vh, new Vector3(-widthHalf, -heigthHalf, rectTransform.position.z), 0);//LB
        AddVert(vh, new Vector3(-widthHalf, heigthHalf, rectTransform.position.z), 1);//LT
        AddVert(vh, new Vector3(widthHalf, heigthHalf, rectTransform.position.z), 2);//RT
        AddVert(vh, new Vector3(widthHalf, -heigthHalf, rectTransform.position.z), 3);//RB

        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(0, 2, 3);
    }

    private void AddVert(VertexHelper vh, Vector3 pos, int index)
    {
        UIVertex cornerVertex = UIVertex.simpleVert;
        cornerVertex.position = pos;
        cornerVertex.color = barColors[index];
        cornerVertex.uv0 = new Vector3(pos.x, pos.y, valueTemp);
        vh.AddVert(cornerVertex);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ProgressBar), true)]
public class ProgressBarEditor : GraphicEditor
{
    SerializedProperty barColors;
    SerializedProperty value;
    SerializedProperty valueGap;
    SerializedProperty txt;
    SerializedProperty solo;

    protected override void OnEnable()
    {
        base.OnEnable();
        barColors = serializedObject.FindProperty("barColors");
        value = serializedObject.FindProperty("value");
        valueGap = serializedObject.FindProperty("valueGap");
        txt = serializedObject.FindProperty("txt");
        solo = serializedObject.FindProperty("solo");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_Material);
        EditorGUILayout.PropertyField(m_RaycastTarget);

        EditorGUILayout.PropertyField(barColors);

        EditorGUILayout.PropertyField(value);
        EditorGUILayout.PropertyField(valueGap);
        EditorGUILayout.PropertyField(txt);
        EditorGUILayout.PropertyField(solo);

        serializedObject.ApplyModifiedProperties();
    }


    //[MenuItem(WidgetEditor.rootMenu + "ProgressBar", false, 103)]
    //public static void CreatePolygonImage()
    //{
    //    var goRoot = Selection.activeGameObject;
    //    if (goRoot == null)
    //        return;
    //    var polygon = new GameObject("ProgressBar");
    //    polygon.AddComponent<ProgressBar>();
    //    polygon.transform.SetParent(goRoot.transform, false);
    //    polygon.transform.SetAsLastSibling();
    //    Undo.RegisterCreatedObjectUndo(polygon, "Created " + polygon.name);
    //}
}
#endif