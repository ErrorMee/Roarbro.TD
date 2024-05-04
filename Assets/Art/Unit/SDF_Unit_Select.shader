Shader "SDF/Unit/Select"
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
                int id = UNITY_ACCESS_INSTANCED_PROP(Props, _Index);

                float2 sdPos = f.uv.xy;
                float sharp = min(_ScreenParams.x, _ScreenParams.y) * 0.1;

                float round = 0.1;
                float th = 0.03;
                float glow = 0.07;
                float sd = opOnion(opRound(sdBox(sdPos, 0.48 - round - th), round), th);

                f.color.rgb *= lerp(1, 0.05, saturate(sd * sharp));
                sd -= glow;

                float alpha = saturate(-sd * sharp * 0.15);
                alpha *= alpha;
                f.color.a *= alpha;
                return f.color;
            }
            ENDHLSL
        }
    }
}