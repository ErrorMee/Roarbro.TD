#if !defined(MY_SDFImage_INCLUDED)
#define MY_SDFImage_INCLUDED
#include "SDF2D.hlsl"
#include "Blend.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

struct appdata
{
    float4 os           : POSITION;
    float4 color        : COLOR;
    float4 uv           : TEXCOORD0;
};

struct v2f
{
    float4 cs           : SV_POSITION;
    float4 color        : COLOR;
    float4 uv           : TEXCOORD0;
    float4 effect       : TEXCOORD1;
    float4 os           : TEXCOORD2;
};

sampler2D _MainTex;
float4 _ClipRect;

v2f vert(appdata v)
{
    v2f f;
    VertexPositionInputs vertexInput;
    vertexInput.positionWS = TransformObjectToWorld(v.os.xyz);
    vertexInput.positionCS = TransformWorldToHClip(vertexInput.positionWS);
    f.cs = vertexInput.positionCS;
    f.color = v.color;

    float uOri = fmod(v.uv.x, 10);
    float vOri = fmod(v.uv.x, 100) - uOri;
    float uDecompress = uOri * 0.5;
    float vDecompress = vOri * 0.05;
    f.uv.xy = float2(uDecompress, vDecompress) - 0.5;//sdfPos
    f.uv.z = (v.uv.x - vOri + uOri) * 0.00162;//sliceRadius use for AA

    float id = fmod(v.uv.y, 100);
    f.uv.w = round(id);//id
    float outLinePre = fmod(v.uv.y, 1000) - id;
    float outline = outLinePre * 0.01;
    float onionPre = fmod(v.uv.y, 10000) - outLinePre - id;
    float onion = onionPre * 0.001;

    float reciprocalRadius = 1.0 / f.uv.z;
    f.effect = float4(
        outline * reciprocalRadius,
        onion * reciprocalRadius, 
        outline * 2.5, reciprocalRadius);

    f.os.xy = v.os.xy; f.os.zw = v.uv.zw;
    return f;
}

float4 sdClipRect(float4 rst, float2 os)
{
#if UNITY_UI_CLIP_RECT
    float errMask = ALessB(_ClipRect.x, -10000);
    _ClipRect.xyzw = (1 - errMask) * _ClipRect.xyzw + float4(-10000, -10000, 10000, 10000) * errMask;
    float2 clipCenter = (_ClipRect.xy + _ClipRect.zw) * 0.5;
    float2 p = os - clipCenter;
    float2 b = abs(_ClipRect.xy - _ClipRect.zw) * 0.5;
    float sd = sdBox(p, b);
    float alpha = saturate(-sd * 0.5);
    rst.a *= alpha;
#endif
    return rst;
}

float4 endSimple(float4 color, float sd, v2f f)
{
    float alpha = saturate(-sd * f.uv.z);
    color.a *= alpha;
    return color;
}

float4 endWithOnion(float4 color, float sd, v2f f)
{
    sd = opOnion(sd, f.effect.y);
    return endSimple(color, sd, f);
}

float4 endWithOnionIn(float4 color, float sd, v2f f)
{
    sd = opOnionIn(sd, f.effect.y);
    return endSimple(color, sd, f);
}

float4 paintIn(float4 colorOri, float sd, v2f f, float4 newColor = 0)
{
    colorOri = lerp(colorOri, newColor, saturate(-sd * f.uv.z));
    return colorOri;
}

float4 endWithOutLine(float4 color, float sd, v2f f, float3 lineColor = 0)
{
    float hesOutline = AGreatB(f.effect.x, 0.0001);
    sd += hesOutline * f.effect.w;
    color.rgb = lerp(color.rgb, lineColor, saturate(sd * f.uv.z));
    sd -= f.effect.x;
    return endSimple(color, sd, f);
}

float4 endWithOnionOutLine(float4 color, float sd, v2f f, float3 lineColor = 0)
{
    sd = opOnion(sd, f.effect.y);
    return endWithOutLine(color, sd, f, lineColor);
}

float4 endWithOutGlow(float4 color, float sd, v2f f, float3 lineColor = 0)
{
    int hasGlow = AGreatB(f.effect.z, 0.0001);
    sd += hasGlow * f.effect.w;
    color.rgb = lerp(color.rgb, lineColor, saturate(sd * f.uv.z));
    sd -= f.effect.z * f.effect.w;

    float alpha = saturate(-sd * f.uv.z / (hasGlow * f.effect.z + (1 - hasGlow)));
    float glowAlpha = (hasGlow) * alpha + (1 - hasGlow); 
    alpha *= glowAlpha; alpha *= alpha;
    color.a *= alpha;
    return color;
}

float4 endWithOnionOutGlow(float4 color, float sd, v2f f, float3 lineColor = 0)
{
    sd = opOnion(sd, f.effect.y);
    return endWithOutGlow(color, sd, f, lineColor);
}
#endif