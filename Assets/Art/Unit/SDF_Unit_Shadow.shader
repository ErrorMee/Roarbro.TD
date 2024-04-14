Shader "SDF/Unit/Shadow"
{
    Properties
    {
        _BaseColor("_BaseColor", Color) = (1, 1, 1, 1)
        _AddColor("_AddColor", Color) = (1, 1, 1, 1)
        [IntRange] _Index("_Index", Range(0, 8)) = 0
    }

        SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "../SDFLib/SDFUnit.hlsl"

            half4 frag(v2f f) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(f);
                float2 sdPos = f.uv.xy; float2 symPos = opSymY(f.uv.xy);
                float sharp = min(_ScreenParams.x, _ScreenParams.y) * 0.1;
                float sketch = sdCircle(sdPos, 0.48) * sharp;
                return endSimple(f.color, sketch);
            }
            ENDHLSL
        }
    }
}