using UnityEngine;

public static class MatPropUtil
{
    static readonly MaterialPropertyBlock materialPropertyBlock = new();

    public static int IndexKey = Shader.PropertyToID("_Index");
    public static int BaseColorKey = Shader.PropertyToID("_BaseColor");
    public static int AddColorKey = Shader.PropertyToID("_AddColor");
    public static int InverseKey = Shader.PropertyToID("_Inverse");
    public static int ProgressKey = Shader.PropertyToID("_Progress");
    public static int OffsetTimeKey = Shader.PropertyToID("_OffsetTime");
    public static int TempRTKey = Shader.PropertyToID("_TempRT");

    private static void ApplyAndClear(MeshRenderer meshRender, bool applyAndClear)
    {
        if (applyAndClear)
        {
            meshRender.SetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.Clear();
        }
    }

    public static void SetMPBFloat(this MeshRenderer self, int key, float value, bool applyAndClear = true)
    {
        materialPropertyBlock.SetFloat(key, value);
        ApplyAndClear(self, applyAndClear);
    }

    public static void SetMPBInt(this MeshRenderer self, int key, int value, bool applyAndClear = true)
    {
        materialPropertyBlock.SetInt(key, value);
        ApplyAndClear(self, applyAndClear);
    }

    public static void SetMPBColor(this MeshRenderer self, int key, Color value, bool applyAndClear = true)
    {
        materialPropertyBlock.SetColor(key, value);
        ApplyAndClear(self, applyAndClear);
    }

    public static void SetMPBTexture(this MeshRenderer self, int key, Texture value, bool applyAndClear = true)
    {
        materialPropertyBlock.SetTexture(key, value);
        ApplyAndClear(self, applyAndClear);
    }
}
