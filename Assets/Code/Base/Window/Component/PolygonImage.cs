using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
#endif
using UnityEngine;
using UnityEngine.UI;

public class PolygonImage : Image
{
    [Range(3, 32)]
    [SerializeField] int count = 4;
    public int Count {
        get { return count; }
        set { count = Mathf.Clamp(value, 3, 32); SetVerticesDirty(); }
    }

    [SerializeField]
    private int radius = 70;
    public int Radius
    {
        get { return radius; }
        set {
            radius = Mathf.Clamp(value, 0, (int)rectTransform.sizeDelta.x);
            SetVerticesDirty();
        }
    }

    [Range(0, 24)]
    [SerializeField] int fade = 4;
    public int Fade
    {
        get { return fade; }
        set
        {
            fade = Mathf.Clamp(value, 0, radius);
            SetVerticesDirty();
        }
    }

    [Range(0, 128)][SerializeField] int hollow = 35;
    public int Hollow
    {
        get { return hollow; }
        set { hollow = Mathf.Clamp(value, 0, 99); }
    }

    [Range(0, 720)]
    [SerializeField] int angle720 = 0;
    public int Angle720
    {
        get { return angle720; }
        set
        {
            angle720 = Mathf.Clamp(value, 0, 720);
            SetVerticesDirty();
        }
    }

    [SerializeField] Color[] modifierColors = new Color[0];

    [SerializeField] Vector2[] modifierPoints = new Vector2[0];

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        for (int i = 0; i < count; i++)
        {
            AddVerts(vh, i);
        }

        if (fade > 0)
        {
            if (hollow > 0)
            {
                for (int i = 0; i < count - 1; i++)
                {
                    int startIndex = i * 4;
                    vh.AddTriangle(startIndex + 0, startIndex + 4, startIndex + 1);
                    vh.AddTriangle(startIndex + 4, startIndex + 5, startIndex + 1);

                    vh.AddTriangle(startIndex + 1, startIndex + 5, startIndex + 2);
                    vh.AddTriangle(startIndex + 5, startIndex + 6, startIndex + 2);

                    vh.AddTriangle(startIndex + 2, startIndex + 6, startIndex + 3);
                    vh.AddTriangle(startIndex + 6, startIndex + 7, startIndex + 3);
                }
                int count4 = count * 4;
                vh.AddTriangle(0, 1, count4 - 4);
                vh.AddTriangle(1, count4 - 3, count4 - 4);

                vh.AddTriangle(1, 2, count4 - 3);
                vh.AddTriangle(2, count4 - 2, count4 - 3);

                vh.AddTriangle(2, 3, count4 - 2);
                vh.AddTriangle(3, count4 - 1, count4 - 2);
            }
            else
            {
                int startIndex;
                int count2 = count * 2;
                for (int i = 0; i < count - 1; i++)
                {
                    startIndex = i * 2 + 1;
                    vh.AddTriangle(startIndex, startIndex - 1, startIndex + 2);
                    vh.AddTriangle(startIndex - 1, startIndex + 1, startIndex + 2);
                    vh.AddTriangle(count2, startIndex, startIndex + 2);
                }
              
                vh.AddTriangle(0, 1, count2 - 1);
                vh.AddTriangle(0, count2 - 1, count2 - 2);
                vh.AddTriangle(1, count2, count2 - 1);
            }
        }
        else
        {
            if (hollow > 0)
            {
                int startIndex;
                for (int i = 0; i < count - 1; i++)
                {
                    startIndex = i * 2;
                    vh.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
                    vh.AddTriangle(startIndex + 1, startIndex + 3, startIndex + 2);
                }
                startIndex = (count - 1) * 2;
                vh.AddTriangle(0, startIndex, startIndex + 1);
                vh.AddTriangle(0, startIndex + 1, 1);
            }
            else
            {
                for (int i = 0; i < count - 1; i++)
                {
                    vh.AddTriangle(count, i, i + 1);
                }
                vh.AddTriangle(count, count - 1, 0);
            }
        }
    }

    private void AddVerts(VertexHelper vh, int index)
    {
        Color verColor = color;
        if (modifierColors.Length > index)
        {
            verColor *= modifierColors[index];
        }
        
        int verAngle = Mathf.RoundToInt(720f / count * index) + angle720;

        float sin = Angle720Util.Sin720(verAngle);
        float cos = Angle720Util.Cos720(verAngle);

        if (fade > 0)
        {
            AddVert(vh, sin, cos, index, radius, verColor, 0);
            AddVert(vh, sin, cos, index, radius - fade, verColor, 1);

            if (hollow > 0)
            {
                AddVert(vh, sin, cos, index, hollow + fade, verColor, 1);
                AddVert(vh, sin, cos, index, hollow, verColor, 0);
            }
            else
            {
                if (index == count - 1)
                {
                    AddVert(vh, sin, cos, index, 0, color, 1);
                }
            }
        }
        else
        {
            AddVert(vh, sin, cos, index, radius, verColor, 1);
            if (hollow > 0)
            {
                AddVert(vh, sin, cos, index, hollow, verColor, 1);
            }
            else
            {
                if (index == count - 1)
                {
                    AddVert(vh, sin, cos, index, 0, color, 1);
                }
            }
        }
    }

    private void AddVert(VertexHelper vh, float sin, float cos, 
        int index, int verRadius, Color verColor, float alpha)
    {
        Vector3 verPos = Vector3.zero;
        verPos.x = sin * verRadius;
        verPos.y = cos * verRadius;

        if (modifierPoints.Length > index)
        {
            Vector2 modifierPoint = modifierPoints[index];
            verPos.x += modifierPoint.x;
            verPos.y += modifierPoint.y;
        }

        Vector2 verUV;
        verUV.x = verPos.x / rectTransform.sizeDelta.x + 0.5f;
        verUV.y = verPos.y / rectTransform.sizeDelta.y + 0.5f;

        UIVertex vertex = UIVertex.simpleVert;
        vertex.position = verPos;
        verColor.a *= alpha;
        vertex.color = verColor;
        vertex.uv0 = verUV;
        vh.AddVert(vertex);
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(PolygonImage), true)]
public class PolygonImageEditor : GraphicEditor
{
    SerializedProperty m_Sprite;
    GUIContent m_SpriteContent;

    SerializedProperty count;
    SerializedProperty radius;
    SerializedProperty fade;
    SerializedProperty hollow;
    SerializedProperty angle720;
    SerializedProperty modifierColors;
    SerializedProperty modifierPoints;
    
    protected override void OnEnable()
    {
        base.OnEnable();

        Angle720Util.Init();

        m_SpriteContent = new GUIContent("Source Image");
        m_Sprite = serializedObject.FindProperty("m_Sprite");

        count = serializedObject.FindProperty("count");
        radius = serializedObject.FindProperty("radius");
        fade = serializedObject.FindProperty("fade");
        hollow = serializedObject.FindProperty("hollow");
        angle720 = serializedObject.FindProperty("angle720");
        modifierColors = serializedObject.FindProperty("modifierColors");
        modifierPoints = serializedObject.FindProperty("modifierPoints");
        
        SetShowNativeSize(true);
    }

    protected override void OnDisable() {}

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SpriteGUI();
        AppearanceControlsGUI();
        RaycastControlsGUI();
        SetShowNativeSize(false);
        NativeSizeButtonGUI();
        MaskGUI();

        serializedObject.ApplyModifiedProperties();
    }

    void SetShowNativeSize(bool instant)
    {
        Image.Type type = Image.Type.Filled;
        bool showNativeSize = (type == Image.Type.Simple || type == Image.Type.Filled) && m_Sprite.objectReferenceValue != null;
        base.SetShowNativeSize(showNativeSize, instant);
    }

    protected void SpriteGUI()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_Sprite, m_SpriteContent);
        EditorGUI.EndChangeCheck();
    }

    protected void MaskGUI()
    {
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(count);
        EditorGUILayout.PropertyField(radius);
        EditorGUILayout.PropertyField(fade);
        EditorGUILayout.PropertyField(hollow);
        EditorGUILayout.PropertyField(angle720);
        EditorGUILayout.PropertyField(modifierColors);
        EditorGUILayout.PropertyField(modifierPoints);
        
        EditorGUI.EndChangeCheck();
    }

    //[MenuItem(WidgetEditor.rootMenu + "Polygon Image", false, 101)]
    //public static void CreatePolygonImage()
    //{
    //    var goRoot = Selection.activeGameObject;
    //    if (goRoot == null)
    //        return;
    //    var polygon = new GameObject("PolygonImage");
    //    PolygonImage polygonImage = polygon.AddComponent<PolygonImage>();
    //    polygonImage.raycastTarget = false;
    //    polygon.transform.SetParent(goRoot.transform, false);
    //    polygon.transform.SetAsLastSibling();
    //    Undo.RegisterCreatedObjectUndo(polygon, "Created " + polygon.name);
    //}
}
#endif