Shader "SDF/UI/Icon"
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
            #include "../SDFLib/SDFImage.hlsl"

            half4 frag(v2f f) : SV_Target
            {
                float2 sdPos = f.uv.xy;
                float id = f.uv.w;
                float radius = 0.4; float roundness = 0.1;
                //if (id == 0)//Clear
                //{
                //    float sd = sdTrapezoid(sdPos - float2(0, -0.05), 0.1, 0.17, 0.18) - roundness;
                //    sd = opUnion(sd, sdCircle(sdPos - float2(0, 0.32), 0));
                //    sd = opSmoothSubtraction(sdSegment(sdPos, float2(-0.5, 0.15), float2(0.5, 0.15)) - 0.13, sd, 0.03);
                //    sd = opSubtraction(sdSegment(opSymY(sdPos), float2(0.16, -0.02), float2(0.12, -0.26)) - 0.13, sd);
                //    return endWithOnionOutGlow(f.color, sd - roundness, f);
                //}
                //if (id == 1)//<
                //{
                //    return f.color;
                //    float th = -(sdPos.x) * 0.3 + 0.15;
                //    float sd = sdOrientedBox(opSymX(sdPos), float2(-0.15, -0.08), float2(0.15, 0.27), th) - roundness;
                //    return endWithOnionOutGlow(f.color, sd, f);
                //}
                if (id == 2)//Lock
                {
                    float sd = sdRoundedBox(sdPos + float2(0, 0.16), float2(0.36, 0.26), roundness);
                    float sd1 = sdTunnel(sdPos - float2(0, 0.15), 0.2);
                    sd = opUnion(opOnion(sd1, 0.062), sd);
                    return endWithOnionOutGlow(f.color, sd, f);
                }
                if (id == 3)//home
                {
                    float sd = sdOrientedBox(opSymY(sdPos), float2(-0.02, 0.27), float2(0.3, 0), 0.01);
                    sd = opUnion(sd, sdBox(float2(sdPos.x, -sdPos.y - 0.12), float2(0.2, 0.2))) - roundness;
                    return endWithOnionOutGlow(f.color, sd, f);
                }
                if (id == 4)//pause
                {
                    float sd = sdOrientedBox(opSymY(sdPos), float2(0.22, 0.25), float2(0.24, -0.27), roundness) - roundness;
                    return endWithOnionOutGlow(f.color, sd, f);
                }

                if (id == 5)//clock
                {
                    float sd = sdCircle(sdPos, 0.4);
                    float cut = sdCircle(sdPos, 0.1);

                    float second = ceil(fmod(_Time.y, 60.0));
                    float sdTime = sdSegment(sdPos, 0, rotate(float2(0, 0.262), -second * PI_DIV_DOZ)) - 0.04;

                    sd = opSubtraction(opUnion(cut, sdTime), sd);
                    return endWithOnionOutGlow(f.color, sd, f);
                }

                if (id == 6) // sound
                {
                    float sd = sdSegment(sdPos, float2(0, 0.3), float2(0.25, 0.2));
                    sd = opUnion(sd, sdSegment(sdPos, float2(0, 0.3), float2(0, -0.2))) - roundness;
                    sd = opUnion(sd, sdCircle(sdPos - float2(-0.1, -0.2), roundness * 2));
                    return endWithOnionOutGlow(f.color, sd, f);
                }

                if (id == 7) // music
                {
                    sdPos -= float2(-0.062, 0);
                    float sd = sdTriangle(sdPos, float2(-0.2, 0), float2(0.062, 0.262), float2(0.062, -0.262));
                    sd = opUnion(sdBox(sdPos - float2(-0.1, 0), 0.12), sd) - roundness;
                    sd = opUnion(sdSegment(opSymX(sdPos), float2(0.34, 0.25), float2(0.3, 0.22)) - 0.062, sd);
                    sd = opUnion(sdSegment(opSymX(sdPos), float2(0.36, 0), float2(0.33, 0)) - 0.062, sd);
                    return sdClipRect(endWithOnionOutGlow(f.color, sd, f), f.os.xy);
                }

                if (id == 8) // Gear
                {
                    float second = ceil(_Time.y * 0.3333);
                    float radian = second * 0.162;
                    sdPos = rotate(sdPos, radian);
                    float sd = sdHexagon(sdPos, 0.38 - roundness) - roundness;
                    sd = opSubtraction(sdCircle(sdPos, 0.11), sd);
                    return endWithOnionOutGlow(f.color, sd, f);
                }

                if (id == 9) // Global
                {
                    roundness *= 0.5;
                    float sd = opOnion(sdCircle(sdPos, 0.4 - roundness), roundness);
                    sd = opUnion(sd, opOnion(sdCircle(opSymY(sdPos) + float2(0.43, 0), 0.62 - roundness), roundness));
                    sd = opUnion(sd, sdSegment(sdPos, float2(-0.3, 0), float2(0.3, 0)) - roundness);
                    sd = opIntersection(sd, sdCircle(sdPos, 0.4));
                    return sdClipRect(endWithOnionOutGlow(f.color, sd, f), f.os.xy);
                }

                if (id == 10) // People
                {
                    float sd = sdCircle(sdPos - float2(0, 0.2), 0.2);
                    sdPos = opSymY(sdPos);
                    sd = opUnion(sd, sdSegment(sdPos, float2(0, -0.05), float2(0.25, 0)) - roundness);
                    sd = opUnion(sd, sdSegment(sdPos, float2(0, -0.1), float2(0.162, -0.3)) - roundness);
                    sd = opUnion(sd, sdSegment(sdPos, float2(0.15, -0.3), float2(0.22, -0.3)) - roundness);
                    return endWithOnionOutGlow(f.color, sd, f);
                }

                if (id == 11)//sort
                {
                    float sd = sdSegment(sdPos, float2(-0.28, 0.28), float2(0.3, 0.28));
                    sd = opUnion(sdSegment(sdPos, float2(-0.28, 0), float2(0.15, 0)), sd);
                    sd = opUnion(sdSegment(sdPos, float2(-0.28, -0.28), float2(0, -0.28)), sd) - roundness;
                    return endWithOnionOutGlow(f.color, sd, f);
                }

                if (id == 12)//pie
                {
                    float sd = sdCircle(sdPos, 0.4);
                    float cut = opUnion(sdSegment(sdPos, float2(0, 0), float2(0.5, 0.1)),
                        sdSegment(sdPos, float2(0, 0), float2(0, 0.5))) - 0.033;
                    sd = opSubtraction(cut, sd);
                    return endWithOnionOutGlow(f.color, sd, f);
                }

                if (id == 13)//wait
                {
                    sdPos.y += 0.03 * sin(_Time.y * 1.62 + sdPos.x * 10);
                    float sd = sdBox(sdPos, float2(0.4, 0.02)) - 0.03;
                    return endWithOnionOutGlow(f.color, sd, f);
                }

                return f.color;
            }
            ENDHLSL
        }
    }
}
