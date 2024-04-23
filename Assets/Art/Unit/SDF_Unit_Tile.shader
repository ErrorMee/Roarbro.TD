Shader "SDF/Unit/Tile"
{
    Properties
    {
        _BaseColor("_BaseColor", Color) = (1, 1, 1, 1)
        _AddColor("_AddColor", Color) = (1, 1, 1, 1)
        [IntRange] _Index("_Index", Range(0, 8)) = 0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry" "RenderPipeline" = "UniversalPipeline" }

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
                float sharp = min(_ScreenParams.x, _ScreenParams.y) * 0.1;
                int index = UNITY_ACCESS_INSTANCED_PROP(Props, _Index);
                float4 addColor = UNITY_ACCESS_INSTANCED_PROP(Props, _AddColor);

                float2 sdPos = abs(f.uv.xy);
                float edge = 0.475 - max(sdPos.x, sdPos.y);

                float sd = edge;

                sd = saturate(-sd * sharp);

                f.color.rgb = lerp(f.color.rgb, addColor.rgb, sd);
                return f.color;
            }
            ENDHLSL
        }
    }
}