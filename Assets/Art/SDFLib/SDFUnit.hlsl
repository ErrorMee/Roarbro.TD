#if !defined(MY_SDFUnit_INCLUDED)
#define MY_SDFUnit_INCLUDED
#include "SDF2D.hlsl"
#include "Blend.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

struct appdata
{
    float4 os           : POSITION;
    float2 uv           : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
    float4 cs           : SV_POSITION;
    float4 color        : COLOR;
    float2 uv           : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

UNITY_INSTANCING_BUFFER_START(Props)
    UNITY_DEFINE_INSTANCED_PROP(float4, _BaseColor)
    UNITY_DEFINE_INSTANCED_PROP(float4, _AddColor)
    UNITY_DEFINE_INSTANCED_PROP(int, _Index)
UNITY_INSTANCING_BUFFER_END(Props)

v2f vert(appdata v)
{
    v2f f;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_TRANSFER_INSTANCE_ID(v, f);

    VertexPositionInputs vertexInput;
    vertexInput.positionWS = TransformObjectToWorld(v.os.xyz);
    vertexInput.positionCS = TransformWorldToHClip(vertexInput.positionWS);
    f.cs = vertexInput.positionCS;
    f.color = UNITY_ACCESS_INSTANCED_PROP(Props, _BaseColor);
    f.uv.xy = v.uv.xy - 0.5;
    return f;
}

float4 paintIn(float4 colorOri, float sd, float4 newColor = 0)
{
    colorOri.rgb = lerp(colorOri.rgb, newColor.rgb, saturate(-sd));
    return colorOri;
}

float4 endSimple(float4 color, float sd)
{
    float alpha = saturate(-sd);
    color.a *= alpha;
    return color;
}

#endif