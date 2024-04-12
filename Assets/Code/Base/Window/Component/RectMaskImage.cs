using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
#endif

public class RectMaskImage : Image
{
    [Range(0, 1)]
    [SerializeField]
    private float m_MaskTop = 0.25f;
    public float MaskTop { get { return m_MaskTop; } set { m_MaskTop = Mathf.Clamp01(value); SetVerticesDirty(); } }
    [Range(0, 1)]
    [SerializeField]
    private float m_MaskBottom = 0.25f;
    public float MaskBottom { get { return m_MaskBottom; } set { m_MaskBottom = Mathf.Clamp01(value); SetVerticesDirty(); } }
    [Range(0, 1)]
    [SerializeField]
    private float m_MaskLeft = 0.25f;
    public float MaskLeft { get { return m_MaskLeft; } set { m_MaskLeft = Mathf.Clamp01(value); SetVerticesDirty(); } }
    [Range(0, 1)]
    [SerializeField]
    private float m_MaskRight = 0.25f;
    public float MaskRight { get { return m_MaskRight; } set { m_MaskRight = Mathf.Clamp01(value); SetVerticesDirty(); } }

    private Color vertColor;

    public void Mask(Vector4 tblr)
    {
        m_MaskTop = Mathf.Clamp01(tblr.x);
        m_MaskBottom = Mathf.Clamp01(tblr.y);
        m_MaskLeft = Mathf.Clamp01(tblr.z);
        m_MaskRight = Mathf.Clamp01(tblr.w);
        SetVerticesDirty();
    }

    [SerializeField, Range(-0.5f, 0.5f)]
    private float m_LeanHorizontal = 0f;
    public float LeanHorizontal { get { return m_LeanHorizontal; } set { m_LeanHorizontal = Mathf.Clamp(value, -0.5f, 0.5f); SetVerticesDirty(); } }

    [Range(-0.5f, 0.5f)]
    [SerializeField]
    private float m_LeanVertical = 0f;
    public float LeanVertical { get { return m_LeanVertical; } set { m_LeanVertical = Mathf.Clamp(value, -0.5f, 0.5f); SetVerticesDirty(); } }

    [SerializeField, Range(0, 0.5f)]
    private float m_FadeOut = 0;
    public float FadeOut
    {
        get { return m_FadeOut; }
        set
        {
            m_FadeOut = Mathf.Clamp(value, 0, m_Radius);
            SetVerticesDirty();
        }
    }

    [Range(-1, 1)]
    [SerializeField]
    private float m_ScrollX = 0f;
    public float ScrollX { get { return m_ScrollX; } set { m_ScrollX = Mathf.Clamp(value, -1, 1); SetVerticesDirty(); } }

    [Range(-1, 1)]
    [SerializeField]
    private float m_ScrollY = 0f;
    public float ScrollY { get { return m_ScrollY; } set { m_ScrollY = Mathf.Clamp(value, -1, 1); SetVerticesDirty(); } }

    public void Scroll(Vector2 xy)
    {
        m_ScrollX = Mathf.Clamp(xy.x, -1, 1);
        m_ScrollY = Mathf.Clamp(xy.y, -1, 1);
        SetVerticesDirty();
    }

    [Range(0, 512)]
    [SerializeField]
    private uint m_Radius = 0;
    public uint Radius
    {
        get { return m_Radius; }
        set
        {
            m_Radius = (uint)Mathf.Clamp(value, 0, 512);
            SetVerticesDirty();
        }
    }

    [Range(2, 128)]
    [SerializeField]
    private uint m_Smoothness = 4;
    public uint Smoothness
    {
        get { return m_Smoothness; }
        set
        {
            m_Smoothness = (uint)Mathf.Clamp(value, 2, 128);
            SetVerticesDirty();
        }
    }

    public bool m_SupportNoneSprite = true;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        float scrollX = m_ScrollX;
        if (scrollX > 0)
        {
            scrollX = scrollX * m_MaskRight;
        }
        if (scrollX < 0)
        {
            scrollX = scrollX * m_MaskLeft;
        }
        float scrollY = m_ScrollY;
        if (scrollY > 0)
        {
            scrollY = scrollY * m_MaskTop;
        }
        if (scrollY < 0)
        {
            scrollY = scrollY * m_MaskBottom;
        }

        Vector4 atlasUV = (overrideSprite != null) ? UnityEngine.Sprites.DataUtility.GetInnerUV(overrideSprite) : Vector4.zero;
        if (sprite == null && !m_SupportNoneSprite)
        {
            vertColor = Color.clear;
        }
        else
        {
            vertColor = this.color;
        }

        if (m_Radius >= m_Smoothness * 2 && Mathf.Abs(m_LeanHorizontal) < 0.01f && Mathf.Abs(m_LeanVertical) < 0.01f)
        {
            RoundQuadFill(vh, scrollX, scrollY, atlasUV);
        }
        else
        {
            QuadFill(vh, scrollX, scrollY, atlasUV);
        }
    }

    private void QuadFill(VertexHelper toFill, float scrollX, float scrollY, Vector4 atlasUV)
    {
        float w = atlasUV.z - atlasUV.x;
        float h = atlasUV.w - atlasUV.y;

        Vector2 fullSize = new Vector2(rectTransform.sizeDelta.x / 2, rectTransform.sizeDelta.y / 2);

        AddQuad(toFill, fullSize, atlasUV, m_MaskLeft + m_FadeOut, m_MaskRight + m_FadeOut, m_MaskBottom + m_FadeOut, m_MaskTop + m_FadeOut, w, h, scrollX, scrollY, 0);

        if (m_FadeOut >= 0.001f)
        {
            vertColor.a = 0;

            UIVertex[] uIVertices = AddQuad(toFill, fullSize, atlasUV, m_MaskLeft, m_MaskRight, m_MaskBottom, m_MaskTop, w, h, scrollX, scrollY, 0, false);
            
            for (int i = 0;i< uIVertices.Length; i++)
            {
                UIVertex uIVertex = uIVertices[i];
                uIVertex.color.a = 0;
            }

            toFill.AddTriangle(4, 0, 3);
            toFill.AddTriangle(3, 7, 4);

            toFill.AddTriangle(7, 3, 2);
            toFill.AddTriangle(2, 6, 7);

            toFill.AddTriangle(6, 2, 1);
            toFill.AddTriangle(1, 5, 6);

            toFill.AddTriangle(5, 1, 0);
            toFill.AddTriangle(0, 4, 5);
        }
    }

    private void RoundQuadFill(VertexHelper toFill, float scrollX, float scrollY, Vector4 atlasUV)
    {
        float w = atlasUV.z - atlasUV.x;
        float h = atlasUV.w - atlasUV.y;

        Vector2 fullSize = new Vector2(rectTransform.sizeDelta.x / 2, rectTransform.sizeDelta.y / 2);

        float radiusXOffset = m_Radius / rectTransform.sizeDelta.x;
        float radiusYOffset = m_Radius / rectTransform.sizeDelta.y;
        float _MaskLeft = m_MaskLeft + radiusXOffset;
        float _MaskRight = m_MaskRight + radiusXOffset;

        UIVertex[] uIVertices1 = AddQuad(toFill, fullSize, atlasUV, _MaskLeft, _MaskRight, m_MaskBottom, m_MaskTop, w, h, scrollX, scrollY, 0);

        float _MaskBottom = m_MaskBottom + radiusYOffset;
        float _MaskTop = m_MaskTop + radiusYOffset;
        UIVertex[] uIVertices2 = AddQuad(toFill, fullSize, atlasUV, m_MaskLeft, 1 - _MaskLeft, _MaskBottom, _MaskTop, w, h, scrollX, scrollY, 1);

        UIVertex[] uIVertices3 = AddQuad(toFill, fullSize, atlasUV, 1 - _MaskRight, m_MaskRight, _MaskBottom, _MaskTop, w, h, scrollX, scrollY, 2);

        uint vertCount = m_Radius / m_Smoothness;

        if (vertCount >= 1)
        {
            AddRound(toFill, vertCount, 0, 4, 7, uIVertices1[0], uIVertices2[0],0);
            AddRound(toFill, vertCount, 5, 1, 6, uIVertices2[1], uIVertices1[1],1);
            AddRound(toFill, vertCount, 2, 10, 9, uIVertices1[2], uIVertices3[2],2);
            AddRound(toFill, vertCount, 11, 3, 8, uIVertices3[3], uIVertices1[3],3);
        }
    }

    private UIVertex[] AddQuad(
        VertexHelper toFill, Vector2 fullSize, Vector4 atlasUV, float _MaskLeft, float _MaskRight, float _MaskBottom, float _MaskTop,
        float w, float h, float scrollX, float scrollY,int quadIndex, bool addTriangle = true)
    {
        float uLeftTop = _MaskLeft + m_LeanHorizontal;
        float xLeftTop = 1 - uLeftTop * 2;

        float uRightTop = _MaskRight - m_LeanHorizontal;
        float xRightTop = 1 - uRightTop * 2;

        float uLeftBottom = _MaskLeft - m_LeanHorizontal;
        float xLeftBottom = 1 - uLeftBottom * 2;

        float uRightBottom = _MaskRight + m_LeanHorizontal;
        float xRightBottom = 1 - uRightBottom * 2;

        float vLetfTop = _MaskTop + m_LeanVertical;
        float yLeftTop = 1 - vLetfTop * 2;

        float vRightTop = _MaskTop - m_LeanVertical;
        float yRightTop = 1 - vRightTop * 2;

        float vLetfBottom = _MaskBottom - m_LeanVertical;
        float yLetfBottom = 1 - vLetfBottom * 2;

        float vRightBottom = _MaskBottom + m_LeanVertical;
        float yRightBottom = 1 - vRightBottom * 2;

        var lb = UIVertex.simpleVert;
        lb.position = new Vector3(-fullSize.x * xLeftBottom, -fullSize.y * yLetfBottom, 0);
        lb.uv0 = new Vector2(atlasUV.x + w * uLeftBottom + scrollX, atlasUV.y + h * vLetfBottom + scrollY);
        lb.color = vertColor;

        var lt = UIVertex.simpleVert;
        lt.position = new Vector3(-fullSize.x * xLeftTop, fullSize.y * yLeftTop, 0);
        lt.uv0 = new Vector2(atlasUV.x + w * uLeftTop + scrollX, atlasUV.w - h * vLetfTop + scrollY);
        lt.color = vertColor;

        var rt = UIVertex.simpleVert;
        rt.position = new Vector3(fullSize.x * xRightTop, fullSize.y * yRightTop, 0);
        rt.uv0 = new Vector2(atlasUV.z - w * uRightTop + scrollX, atlasUV.w - h * vRightTop + scrollY);
        rt.color = vertColor;

        var rb = UIVertex.simpleVert;
        rb.position = new Vector3(fullSize.x * xRightBottom, -fullSize.y * yRightBottom, 0);
        rb.uv0 = new Vector2(atlasUV.z - w * uRightBottom + scrollX, atlasUV.y + h * vRightBottom + scrollY);
        rb.color = vertColor;

        toFill.AddVert(lb);
        toFill.AddVert(lt);
        toFill.AddVert(rt);
        toFill.AddVert(rb);

        if (addTriangle)
        {
            int offsetIndex = quadIndex * 4;
            toFill.AddTriangle(0 + offsetIndex, 1 + offsetIndex, 2 + offsetIndex);
            toFill.AddTriangle(2 + offsetIndex, 3 + offsetIndex, 0 + offsetIndex);
        }

        UIVertex[] uIVertices = new UIVertex[4]{ lb, lt, rt, rb };
        return uIVertices;
    }

    private void AddRound(VertexHelper toFill, uint vertCount, int firstIndex, int secondIndex, int centerIndex, 
        UIVertex firstVert, UIVertex secondVert, int roundIndex)
    {
        int[] outVertIndexs = new int[vertCount];
        outVertIndexs[0] = firstIndex;
        outVertIndexs[outVertIndexs.Length - 1] = secondIndex;

        int vertIndex = 11 + roundIndex * ((int)vertCount - 2);
        float angleGap = Mathf.PI / 2 / (outVertIndexs.Length - 1);

        float uRadius = m_Radius / rectTransform.sizeDelta.x;
        float vRadius = m_Radius / rectTransform.sizeDelta.y;

        for (int i = 1; i < vertCount - 1; i++)
        {
            vertIndex++;
            outVertIndexs[i] = vertIndex;

            UIVertex vert = UIVertex.simpleVert;
            
            float crtAngle = angleGap * i;

            float sin = Mathf.Sin(crtAngle);//0 - 1
            float cos = Mathf.Cos(crtAngle);//1 - 0

            if (firstVert.position.x > secondVert.position.x)
            {
                if (firstVert.position.y > secondVert.position.y)
                {
                    vert.position = new Vector3(secondVert.position.x + cos * m_Radius, firstVert.position.y - sin * m_Radius, 0);
                    vert.uv0 = new Vector2(secondVert.uv0.x + cos * uRadius, firstVert.uv0.y - sin * vRadius);
                }
                else
                {
                    vert.position = new Vector3(firstVert.position.x - sin * m_Radius, secondVert.position.y - cos * m_Radius, 0);
                    vert.uv0 = new Vector2(firstVert.uv0.x - sin * uRadius, secondVert.uv0.y - cos * vRadius);
                }
            }
            else
            {
                if (firstVert.position.y > secondVert.position.y)
                {
                    vert.position = new Vector3(firstVert.position.x + sin * m_Radius, secondVert.position.y + cos * m_Radius, 0);
                    vert.uv0 = new Vector2(firstVert.uv0.x + sin * uRadius, secondVert.uv0.y + cos * vRadius);
                }
                else
                {
                    vert.position = new Vector3(secondVert.position.x - cos * m_Radius, firstVert.position.y + sin * m_Radius, 0);
                    vert.uv0 = new Vector2(secondVert.uv0.x - cos * uRadius, firstVert.uv0.y + sin * vRadius);
                }
            }

            vert.color = vertColor;
            toFill.AddVert(vert);
        }

        for (int i = 1; i < vertCount; i++)
        {
            toFill.AddTriangle(outVertIndexs[i - 1], outVertIndexs[i], centerIndex);
        }
    }

    public void SetSquareThumbnail(float sideLength = 0)
    {
        if (sideLength == 0)
        {
            sideLength = Mathf.Min(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
        }

        if (sprite == null && overrideSprite == null)
        {
            MaskLeft = 0;
            MaskRight = 0;
            MaskTop = 0;
            MaskBottom = 0;
            rectTransform.sizeDelta = new Vector2(sideLength, sideLength);
            return;
        }


        Sprite showSprite;
        if (overrideSprite != null)
        {
            showSprite = overrideSprite;
        }
        else
        {
            showSprite = sprite;
        }


        if (showSprite.rect.width > showSprite.rect.height)
        {
            float maskValue = 0.5f * (showSprite.rect.width - showSprite.rect.height) / showSprite.rect.width;
            float ratio = showSprite.rect.width / showSprite.rect.height;
            MaskLeft = maskValue;
            MaskRight = maskValue;
            MaskTop = 0;
            MaskBottom = 0;
            rectTransform.sizeDelta = new Vector2(sideLength * ratio, sideLength);
        }
        else if (showSprite.rect.width < showSprite.rect.height)
        {
            float maskValue = 0.5f * (showSprite.rect.height - showSprite.rect.width) / showSprite.rect.height;
            float ratio = showSprite.rect.height / showSprite.rect.width;
            MaskLeft = 0;
            MaskRight = 0;
            MaskTop = maskValue;
            MaskBottom = maskValue;
            rectTransform.sizeDelta = new Vector2(sideLength, sideLength * ratio);
        }
        else
        {
            MaskLeft = 0;
            MaskRight = 0;
            MaskTop = 0;
            MaskBottom = 0;
            rectTransform.sizeDelta = new Vector2(sideLength, sideLength);
        }
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(RectMaskImage), true)]
[CanEditMultipleObjects]
public class RectMaskImageEditor : GraphicEditor
{
    SerializedProperty m_Sprite;
    GUIContent m_SpriteContent;

    SerializedProperty m_MaskTop, m_MaskBottom, m_MaskLeft, m_MaskRight;
    SerializedProperty m_LeanHorizontal, m_LeanVertical;
    SerializedProperty m_FadeOut;
    SerializedProperty m_ScrollX, m_ScrollY;
    SerializedProperty m_Radius, m_Smoothness;
    SerializedProperty m_SupportNoneSprite;

    protected override void OnEnable()
    {
        base.OnEnable();

        m_SpriteContent = new GUIContent("Source Image");

        m_Sprite = serializedObject.FindProperty("m_Sprite");

        m_MaskTop = serializedObject.FindProperty("m_MaskTop");
        m_MaskBottom = serializedObject.FindProperty("m_MaskBottom");
        m_MaskLeft = serializedObject.FindProperty("m_MaskLeft");
        m_MaskRight = serializedObject.FindProperty("m_MaskRight");
        m_LeanHorizontal = serializedObject.FindProperty("m_LeanHorizontal");
        m_LeanVertical = serializedObject.FindProperty("m_LeanVertical");
        m_FadeOut = serializedObject.FindProperty("m_FadeOut");
        m_ScrollX = serializedObject.FindProperty("m_ScrollX");
        m_ScrollY = serializedObject.FindProperty("m_ScrollY");
        m_Radius = serializedObject.FindProperty("m_Radius");
        m_Smoothness = serializedObject.FindProperty("m_Smoothness");

        m_SupportNoneSprite = serializedObject.FindProperty("m_SupportNoneSprite");
        SetShowNativeSize(true);
    }

    protected override void OnDisable()
    {

    }

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

        EditorGUILayout.PropertyField(m_MaskTop);
        EditorGUILayout.PropertyField(m_MaskBottom);
        EditorGUILayout.PropertyField(m_MaskLeft);
        EditorGUILayout.PropertyField(m_MaskRight);
        EditorGUILayout.PropertyField(m_LeanHorizontal);
        EditorGUILayout.PropertyField(m_LeanVertical);
        EditorGUILayout.PropertyField(m_FadeOut);
        EditorGUILayout.PropertyField(m_ScrollX);
        EditorGUILayout.PropertyField(m_ScrollY);
        EditorGUILayout.PropertyField(m_Radius);
        EditorGUILayout.PropertyField(m_Smoothness);

        EditorGUILayout.PropertyField(m_SupportNoneSprite);

        EditorGUI.EndChangeCheck();
    }

    //[MenuItem(WidgetEditor.rootMenu + "RectMask Image", false, 102)]
    //public static void CreateMaskImage()
    //{
    //    var goRoot = Selection.activeGameObject;
    //    if (goRoot == null)
    //        return;
    //    var polygon = new GameObject("RectMaskImage");
    //    polygon.AddComponent<RectMaskImage>();
    //    polygon.transform.SetParent(goRoot.transform, false);
    //    polygon.transform.SetAsLastSibling();
    //    Undo.RegisterCreatedObjectUndo(polygon, "Created " + polygon.name);
    //}
}
#endif
