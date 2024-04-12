Shader "SDF/UI/Area"
{
    Properties
    {
        [HideInInspector] [NoScaleOffset] _MainTex("_MainTex", 2D) = "white" {}
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
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #include "../SDFLib/SDFImage.hlsl"

            half4 frag(v2f f) : SV_Target
            {
                float2 sdPos = f.uv.xy;
                float id = f.uv.w;
                float radius = 0.4; float roundness = 0.1;
                if (id == 0)
                {
                    float sd = sdCircle(sdPos, radius);
                    return endWithOnionOutGlow(f.color, sd, f);
                }
                if (id == 1)
                {
                    float sd = sdQuadraticCircle(sdPos, radius);
                    return sdClipRect(endWithOnionOutGlow(f.color, sd, f), f.os.xy);
                }
                if (id == 2)
                {
                    float round = 0.2; float skew = 0.1; radius -= round;
                    float sd = sdParallelogram(sdPos, radius - skew, radius, skew) - round;
                    return endWithOnionOutGlow(f.color, sd, f);
                }
                if (id == 3)
                {
                    float round = 0.2; radius -= round;
                    float sd = sdTrapezoid(sdPos, radius, radius - round, radius) - round;
                    return endWithOnionOutGlow(f.color, sd, f);
                }
                if (id == 4)
                {
                    float round = 0.2;
                    float sd = sdRhombus(sdPos, radius - round) - round;
                    return endWithOnionOutGlow(f.color, sd, f);
                }
                if (id == 5)
                {
                    float round = 0.2; 
                    float box = sdBox(sdPos + float2(round - 0.1, 0), float2(0.1, radius - round));
                    float sd = opUnion(sdRhombus(sdPos, radius - round), box) - round;
                    return endWithOnionOutGlow(f.color, sd, f);
                }

                if (id == 6)
                {
                    float sd = sdRoundedBox(sdPos, radius, half4(radius, roundness, roundness, roundness));
                    return sdClipRect(endWithOnionOutGlow(f.color, sd, f), f.os.zw);
                }
                if (id == 7)
                {
                    float sd = sdRoundedBox(sdPos, radius, half4(radius, radius, roundness, roundness));
                    return endWithOnionOutGlow(f.color, sd, f);
                }
                if (id == 8)
                {
                    float sd = sdRoundedBox(sdPos, radius, half4(radius, radius, radius, roundness));
                    return endWithOnionOutGlow(f.color, sd, f);
                }

                if (id == 9)
                {
                    float sd = sdParallelogram(opSymX(sdPos) - float2(0, 0.15), 0.11, 0.14, 0.1) - roundness;
                    return endWithOnionOutGlow(f.color, sd, f);
                }

                if (id == 10)
                {
                    float sd = sdBox(sdPos, float2(0.32, (0.5 - abs(sdPos.x)) * 0.08 + 0.04));
                    sd = opUnion(sd, sdBox(sdPos, float2((0.5 - abs(sdPos.y)) * 0.08 + 0.04, 0.32)));
                    return endWithOnionOutGlow(f.color, sd - roundness, f);
                }

                if (id == 11)
                {
                    float sd = sdBox(sdPos, 0.26) - roundness;
                    float cut = sdCircle(abs(sdPos) - roundness * 1.5, roundness * 0.8);
                    sd = opSubtraction(cut, sd);
                    return endWithOnionOutGlow(f.color, sd, f);
                }
                return f.color;
            }
            ENDHLSL
        }
    }
}
