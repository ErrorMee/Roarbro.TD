
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
[AddComponentMenu("SDFUI/SDFImg", 1)]
public class SDFImg : MaskableGraphic
{
    #region prop
    public enum SliceMode : byte
    {
        One, Three, Nine,
    }

    [SerializeField] SliceMode slice = SliceMode.One;
    public SliceMode Slice
    {
        get { return slice; }
        set
        {
            if (slice != value)
            {
                slice = value; SetVerticesDirty();
            }
        }
    }

    [SerializeField] int sliceRadius = 32;
    public int SliceRadius
    {
        get { return sliceRadius; }
        set
        {
            float minEdge = Mathf.Min(rectTransform.rect.size.x, rectTransform.rect.size.y);
            int maxRadius = Mathf.CeilToInt(minEdge * 0.5f);
            SetProp(value, ref sliceRadius, maxRadius);
        }
    }

    public const int ID_MAX = 100;
    [SerializeField] int id;
    public int ID
    {
        get { return id; }
        set { SetProp(value, ref id, ID_MAX); }
    }

    public const int EFFECT_MAX = 10;
    [SerializeField] int outline;
    public int Outline
    {
        get { return outline; }
        set { SetProp(value, ref outline, EFFECT_MAX); }
    }

    [SerializeField] int onion;
    public int Onion
    {
        get { return onion; }
        set { SetProp(value, ref onion, EFFECT_MAX); }
    }

    private void SetProp(int value, ref int propValue, int maxValue)
    {
        int clampValue = Mathf.Clamp(value, 0, maxValue - 1);
        if (propValue != clampValue)
        {
            propValue = clampValue; SetVerticesDirty();
        }
    }

    #endregion

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear(); //Debug.LogError("OnPopulateMesh");
        GenarateMesh(vh);
    }

    private void GenarateMesh(VertexHelper vh)
    {
        bool xSliced = false; bool ySliced = false;
        float sliceRadius = this.sliceRadius;

        switch (slice)
        {
            case SliceMode.Three:
                float xMinusY = rectTransform.rect.size.x - rectTransform.rect.size.y;
                if (xMinusY > 1f)
                {
                    xSliced = true; ySliced = false;
                    sliceRadius = rectTransform.rect.size.y * 0.5f;
                }
                else if (xMinusY < -1f)
                {
                    xSliced = false; ySliced = true;
                    sliceRadius = rectTransform.rect.size.x * 0.5f;
                }
                break;
            case SliceMode.Nine:
                float minEdge = Mathf.Min(rectTransform.rect.size.x, rectTransform.rect.size.y);
                sliceRadius = Mathf.Min(minEdge * 0.5f, sliceRadius);

                float radius2 = sliceRadius * 2;

                xSliced = (rectTransform.rect.size.x - radius2) > 0.1f;
                ySliced = (rectTransform.rect.size.y - radius2) > 0.1f;
                break;
        }

        Vector2 halfSize = rectTransform.rect.size * 0.5f;

        if (xSliced)
        {
            if (ySliced)
            {
                AddY3Rect(vh, 0, sliceRadius, new Vector2(-halfSize.x, -halfSize.y), new Vector2(-halfSize.x + sliceRadius, -halfSize.y + sliceRadius),
                   new Vector2(-halfSize.x + sliceRadius, halfSize.y - sliceRadius), new Vector2(-halfSize.x + sliceRadius, halfSize.y),
                   Vector2.zero, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 1));

                AddY3Rect(vh, 1, sliceRadius, new Vector2(-halfSize.x + sliceRadius, -halfSize.y), new Vector2(halfSize.x - sliceRadius, -halfSize.y + sliceRadius),
                   new Vector2(halfSize.x - sliceRadius, halfSize.y - sliceRadius), new Vector2(halfSize.x - sliceRadius, halfSize.y),
                   new Vector2(0.5f, 0), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 1));

                AddY3Rect(vh, 2, sliceRadius, new Vector2(halfSize.x - sliceRadius, -halfSize.y), new Vector2(halfSize.x, -halfSize.y + sliceRadius),
                   new Vector2(halfSize.x, halfSize.y - sliceRadius), new Vector2(halfSize.x, halfSize.y),
                   new Vector2(0.5f, 0), new Vector2(1, 0.5f), new Vector2(1, 0.5f), new Vector2(1, 1));
            }
            else
            {
                AddX3Rect(vh, 0, new Vector2(-halfSize.x, -halfSize.y), new Vector2(-halfSize.x + sliceRadius, halfSize.y),
                    new Vector2(halfSize.x - sliceRadius, halfSize.y), new Vector2(halfSize.x, halfSize.y),
                    Vector2.zero, new Vector2(0.5f, 1), new Vector2(0.5f, 1), Vector2.one);
            }
        }
        else
        {
            if (ySliced)
            {
                AddY3Rect(vh, 0, halfSize.x, new Vector2(-halfSize.x, -halfSize.y), new Vector2(halfSize.x, -halfSize.y + sliceRadius),
                    new Vector2(halfSize.x, halfSize.y - sliceRadius), new Vector2(halfSize.x, halfSize.y),
                    Vector2.zero, new Vector2(1, 0.5f), new Vector2(1, 0.5f), Vector2.one);
            }
            else
            {
                AddRect(vh, 0, Mathf.Min(halfSize.x, halfSize.y), new Vector2(-halfSize.x, -halfSize.y), new Vector2(halfSize.x, halfSize.y),
                    Vector2.zero, Vector2.one);
            }
        }
    }

    private void AddX3Rect(VertexHelper vh, int index,
        Vector2 leftDownPos1, Vector2 rightUpPos1, Vector2 rightUpPos2, Vector2 rightUpPos3,
        Vector2 leftDownUV1, Vector2 rightUpUV1, Vector2 rightUpUV2, Vector2 rightUpUV3)
    {
        int indexOffset = index * 3;
        AddRect(vh, 0 + indexOffset, rightUpPos1.y, leftDownPos1, rightUpPos1, leftDownUV1, rightUpUV1);
        AddRect(vh, 1 + indexOffset, rightUpPos1.y, new(rightUpPos1.x, leftDownPos1.y), rightUpPos2,
            new(rightUpUV1.x, leftDownUV1.y), rightUpUV2);
        AddRect(vh, 2 + indexOffset, rightUpPos1.y, new(rightUpPos2.x, leftDownPos1.y), rightUpPos3,
            new(rightUpUV2.x, leftDownUV1.y), rightUpUV3);
    }

    private void AddY3Rect(VertexHelper vh, int index, float slicedRadius,
        Vector2 leftDownPos1, Vector2 rightUpPos1, Vector2 rightUpPos2, Vector2 rightUpPos3,
        Vector2 leftDownUV1, Vector2 rightUpUV1, Vector2 rightUpUV2, Vector2 rightUpUV3)
    {
        int indexOffset = index * 3;
        AddRect(vh, 0 + indexOffset, slicedRadius, leftDownPos1, rightUpPos1, leftDownUV1, rightUpUV1);
        AddRect(vh, 1 + indexOffset, slicedRadius, new(leftDownPos1.x, rightUpPos1.y), rightUpPos2,
            new(leftDownUV1.x, rightUpUV1.y), rightUpUV2);
        AddRect(vh, 2 + indexOffset, slicedRadius, new(leftDownPos1.x, rightUpPos2.y), rightUpPos3,
            new(leftDownUV1.x, rightUpUV2.y), rightUpUV3);
    }

    private void AddRect(VertexHelper vh, int index, float slicedRadius,
        Vector2 leftDownPos, Vector2 rightUpPos, Vector2 leftDownUV, Vector2 rightUpUV)
    {
        UIVertex vert = UIVertex.simpleVert; vert.color = color;
        slicedRadius = ((int)(slicedRadius * 10)) * 100;

        int compressEffects = id + outline * 100 + onion * 1000;

        vert.position = new Vector2(leftDownPos.x, leftDownPos.y);
        vert.uv0 = new Vector4(CompressUVAndRadius(leftDownUV.x, leftDownUV.y, slicedRadius), compressEffects, leftDownPos.x, leftDownPos.y);
        vh.AddVert(vert);//leftDown

        vert.position = new Vector2(leftDownPos.x, rightUpPos.y);
        vert.uv0 = new Vector4(CompressUVAndRadius(leftDownUV.x, rightUpUV.y, slicedRadius), compressEffects, leftDownPos.x, rightUpPos.y);
        vh.AddVert(vert);//leftUp

        vert.position = rightUpPos;
        vert.uv0 = new Vector4(CompressUVAndRadius(rightUpUV.x, rightUpUV.y, slicedRadius), compressEffects, rightUpPos.x, rightUpPos.y);
        vh.AddVert(vert);//rightUp

        vert.position = new Vector2(rightUpPos.x, leftDownPos.y);
        vert.uv0 = new Vector4(CompressUVAndRadius(rightUpUV.x, leftDownUV.y, slicedRadius), compressEffects, rightUpPos.x, leftDownPos.y);
        vh.AddVert(vert);//rightDown

        int indexOffset = index * 4;
        vh.AddTriangle(0 + indexOffset, 1 + indexOffset, 2 + indexOffset);
        vh.AddTriangle(2 + indexOffset, 3 + indexOffset, 0 + indexOffset);
    }

    private int CompressUVAndRadius(float u, float v, float radius)
    {
        return Mathf.RoundToInt(u * 2 + v * 20 + radius);
    }
}