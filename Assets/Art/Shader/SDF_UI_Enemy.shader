Shader "SDF/UI/Enemy"
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

            float4 end(v2f f, float sd)
            {
                return sdClipRect(endWithOnionOutGlow(f.color, sd, f), f.os.xy);
            }

            half4 frag(v2f f) : SV_Target
            {
                float2 sdPos = f.uv.xy;
                float2 sdPosSymY = opSymY(sdPos);
                float id = f.uv.w;
                float radius = 0.4; float roundness = 0.1;
                float animatX = (abs(frac(_Time.y * 1.33) - 0.5) * 2 - 0.5);

                if (id == 0)//Clear
                {
                    float sd = sdTrapezoid(sdPos - float2(0, -0.05), 0.1, 0.17, 0.18) - roundness;
                    sd = opUnion(sd, sdCircle(sdPos - float2(0, 0.32), 0));
                    sd = opSmoothSubtraction(sdSegment(sdPos, float2(-0.5, 0.15), float2(0.5, 0.15)) - 0.13, sd, 0.03);
                    sd = opSubtraction(sdSegment(opSymY(sdPos), float2(0.16, -0.02), float2(0.12, -0.26)) - 0.13, sd);
                    return end(f, sd - roundness);
                }
                if (id == 1)
                {
                    float sd1 = sdTriangle(sdPosSymY, float2(0, -0.2),
                        float2(0.33 + animatX * 0.02, -animatX * 0.02),
                        float2(0.01, 0.33)) - 0.1;

                    float sd = sdBezier(sdPosSymY, float2(0.125, -0.15), float2(0.175, -0.3),
                        float2(0.23 + 0.08 * animatX, -0.32 + 0.08 * animatX));
                    float sd3 = sdBezier(sdPosSymY, float2(0.025, -0.15), float2(0.04, -0.325),
                        float2(0.07 + 0.06 * animatX, -0.4));

                    sd = opUnion(sd, sd3) - 0.06 - sdPosSymY.y * 0.05;
                    sd = opUnion(opUnion(sd1, sd), sdCircle(sdPosSymY - float2(0, 0.05), 0.3));

                    return end(f, sd);
                }
                if (id == 2)
                {
                    float sd1 = sdQuadraticCircle(sdPosSymY + float2(0, -0.1), 0.33 + animatX * 0.01);
                    float sd = sdBezier(sdPosSymY, float2(0.125, -0.15), float2(0.175, -0.3),
                        float2(0.23 + 0.08 * animatX, -0.32 + 0.08 * animatX));
                    float sd3 = sdBezier(sdPosSymY, float2(0.025, -0.15), float2(0.04, -0.325),
                        float2(0.07 + 0.06 * animatX, -0.4));

                    sd = opUnion(sd, sd3) - 0.06 - sdPosSymY.y * 0.05;
                    sd = opUnion(sd1, sd);

                    return end(f, sd);
                }

                if (id == 3)
                {
                    float sd = sdCircle(sdPosSymY + float2(0, -0.162), 0.12);
                    float sd1 = sdTriangle(sdPosSymY, float2(0.05, -0.16),
                        float2(0.27 + animatX * 0.05, 0), float2(0.1, 0.23));

                    sd = opUnion(sd1, sd) - 0.14;
                    sd = opUnion(sd,
                        sdSegment(sdPos, float2(0, -0.23), float2(animatX * 0.1, -0.43 + abs(animatX * 0.05))) - 0.12 - sdPos.y * 0.2);

                    return end(f, sd);
                }

                if (id == 4)
                {
                    float sd = sdQuadraticCircle(sdPosSymY + float2(0, 0.062), 0.28);
                    float sd1 = sdCircle(sdPosSymY - float2(0.23, 0.23), 0.12);
                    sd1 = opSubtraction(sdTriangle(sdPosSymY, float2(0.22, 0.22),
                        float2(1 + sign(sdPos.x) * animatX * 0.6, 0.7), float2(0.36 - sign(sdPos.x) * animatX * 0.4, 1)), sd1);

                    sd = opUnion(sd1, sd);
                    float move = animatX * 0.025 * sign(sdPos.x);
                    sd = opUnion(sdSegment(sdPosSymY, float2(0.25, 0), float2(0.38, -0.03 + move)) - 0.05, sd);
                    sd = opUnion(sdSegment(sdPosSymY, float2(0.25, -0.17), float2(0.33, -0.22 - move)) - 0.05, sd);

                    return end(f, sd);
                }

                if (id == 5)
                {
                    float sd = sdStar5(sdPos, 0.39, 0.5 + animatX * 0.1) - 0.05;

                    return end(f, sd);
                }

                if (id == 6)
                {
                    float sd = sdCircle(sdPosSymY + float2(0.1, -0.05), 0.4);

                    sd = opSubtraction(sdTriangle(sdPos, float2(-0.25 - 0.2 * animatX, 0.7),
                        float2(0.25 + 0.2 * animatX, 0.7), float2(0, 0.25)), sd);

                    float sd2 = opSmoothSubtraction(sdCircle(sdPos + float2(animatX * 0.1, 0.55), 0.17),
                        sdCircle(sdPos + float2(animatX * 0.05, 0.36), 0.1), 0.02);

                    sd = opUnion(sd, sd2);

                    return end(f, sd);
                }

                if (id == 7)
                {
                    float sd = sdEllipse(sdPos - float2(0, 0.22), float2(0.3, 0.16));
                    sd = opUnion(sd, sdSegment(sdPosSymY, float2(0, 0.22), float2(0, -0.27 + animatX * 0.1)) - 0.12);
                    sd = opUnion(sd, sdSegment(sdPosSymY, float2(0, 0.22), float2(0.2, -0.20 + animatX * 0.07)) - 0.12);
                    sd = opUnion(sd, sdSegment(sdPosSymY, float2(0.1, 0.22), float2(0.3, -0.03 + animatX * 0.05)) - 0.12);

                    return end(f, sd);
                }

                return f.color;
            }
            ENDHLSL
        }
    }
}
