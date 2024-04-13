Shader "SDF/UI/Ascii01"
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
                float thickness = 0.09;
                float2 sdPos = f.uv.xy * float2(0.75, 1);
                float offsetY = 0.42 - thickness;
                float offsetX = offsetY * 0.6;
                float id = f.uv.w;

                if (id == 43)//+
                {
                    float sd = sdSegment(sdPos, float2(-offsetX, 0), float2(offsetX, 0)) - thickness;
                    sd = opUnion(sd, sdSegment(sdPos, float2(0, -offsetX), float2(0, offsetX)) - thickness);
                    return endSimple(f.color, sd, f);
                }

                if (id == 45)//-
                {
                    float sd = sdSegment(sdPos, float2(-offsetX, 0), float2(offsetX, 0)) - thickness;
                    return endSimple(f.color, sd, f);
                }

                if (id == 47)///
                {
                    float offsetx = offsetX * 0.62;
                    float sd = sdSegment(sdPos, float2(-offsetx, -offsetX), float2(offsetx, offsetX)) - thickness;
                    return endSimple(f.color, sd, f);
                }

                if (id == 48)//0
                {
                    float sd = sdSegment(opSymX(sdPos), float2(-offsetX, offsetY), float2(offsetX, offsetY)) - thickness;
                    sd = opUnion(sd, sdSegment(opSymY(sdPos), float2(offsetX, -offsetY), float2(offsetX, offsetY)) - thickness);
                    return endSimple(f.color, sd, f);
                }
                if (id == 49)//1
                {
                    float sd = sdSegment(sdPos, float2(-offsetX * 0.62, offsetY), float2(0, offsetY)) - thickness;
                    sd = opUnion(sd, sdSegment(sdPos, float2(0, -offsetY), float2(0, offsetY)) - thickness);
                    sd = opUnion(sd, sdSegment(sdPos, float2(-offsetX, -offsetY), float2(offsetX, -offsetY)) - thickness);
                    return endSimple(f.color, sd, f);
                }
                if (id == 50)//2
                {
                    float sd = sdSegment(opSymX(sdPos), float2(-offsetX, offsetY), float2(offsetX, offsetY)) - thickness;
                    sd = opUnion(sd, sdSegment(sdPos, float2(-offsetX, 0), float2(offsetX, 0)) - thickness);
                    sd = opUnion(sd, sdSegment(sdPos * sign(sign(sdPos.x) + sign(sdPos.y)), float2(offsetX, 0), float2(offsetX, offsetY)) - thickness);
                    return endSimple(f.color, sd, f);
                }
                if (id == 51)//3
                {
                    float sd = sdSegment(opSymX(sdPos), float2(-offsetX, offsetY), float2(offsetX, offsetY)) - thickness;
                    sd = opUnion(sd, sdSegment(sdPos, float2(-offsetX, 0), float2(offsetX, 0)) - thickness);
                    sd = opUnion(sd, sdSegment(sdPos, float2(offsetX, -offsetY), float2(offsetX, offsetY)) - thickness);
                    return endSimple(f.color, sd, f);
                }
                if (id == 52)//4
                {
                    float sd = sdSegment(sdPos, float2(-offsetX, 0), float2(offsetX, 0)) - thickness;
                    sd = opUnion(sd, sdSegment(sdPos, float2(-offsetX * 0.8, offsetY), float2(-offsetX, 0)) - thickness);
                    sd = opUnion(sd, sdSegment(sdPos, float2(offsetX, -offsetY), float2(offsetX, offsetY)) - thickness);
                    return endSimple(f.color, sd, f);
                }
                if (id == 53)//5
                {
                    float sd = sdSegment(opSymX(sdPos), float2(-offsetX, offsetY), float2(offsetX, offsetY)) - thickness;
                    sd = opUnion(sd, sdSegment(sdPos, float2(-offsetX, 0), float2(offsetX, 0)) - thickness);
                    sd = opUnion(sd, sdSegment(sdPos, float2(-offsetX, 0), float2(-offsetX, offsetY)) - thickness);
                    sd = opUnion(sd, sdSegment(sdPos, float2(offsetX, 0), float2(offsetX, -offsetY)) - thickness);
                    return endSimple(f.color, sd, f);
                }
                if (id == 54)//6
                {
                    float sd = sdSegment(opSymX(sdPos), float2(-offsetX, offsetY), float2(offsetX, offsetY)) - thickness;
                    sd = opUnion(sd, sdSegment(sdPos, float2(-offsetX, 0), float2(offsetX, 0)) - thickness);
                    sd = opUnion(sd, sdSegment(sdPos, float2(-offsetX, -offsetY), float2(-offsetX, offsetY)) - thickness);
                    sd = opUnion(sd, sdSegment(sdPos, float2(offsetX, 0), float2(offsetX, -offsetY)) - thickness);
                    return endSimple(f.color, sd, f);
                }
                if (id == 55)//7
                {
                    float sd = sdSegment(sdPos, float2(0, -offsetY), float2(offsetX, offsetY)) - thickness;
                    sd = opUnion(sd, sdSegment(sdPos, float2(-offsetX, offsetY), float2(offsetX, offsetY)) - thickness);
                    return endSimple(f.color, sd, f);
                }
                if (id == 56)//8
                {
                    float sd = sdSegment(opSymX(sdPos), float2(-offsetX, offsetY), float2(offsetX, offsetY)) - thickness;
                    sd = opUnion(sd, sdSegment(opSymY(sdPos), float2(offsetX, -offsetY), float2(offsetX, offsetY)) - thickness);
                    sd = opUnion(sd, sdSegment(sdPos, float2(-offsetX, 0), float2(offsetX, 0)) - thickness);
                    return endSimple(f.color, sd, f);
                }
                if (id == 57)//9
                {
                    float sd = sdSegment(opSymX(sdPos), float2(-offsetX, offsetY), float2(offsetX, offsetY)) - thickness;
                    sd = opUnion(sd, sdSegment(sdPos, float2(-offsetX, 0), float2(offsetX, 0)) - thickness);
                    sd = opUnion(sd, sdSegment(sdPos, float2(offsetX, -offsetY), float2(offsetX, offsetY)) - thickness);
                    sd = opUnion(sd, sdSegment(sdPos, float2(-offsetX, 0), float2(-offsetX, offsetY)) - thickness);
                    return endSimple(f.color, sd, f);
                }

                if (id == 65)//A
                {
                    float sd = opOnion(sdRoundedBox(sdPos - float2(0, -0.2), float2(0.22, 0.51), 0.2), 0.001);
                    sd = opSmoothSubtraction(sdSegment(sdPos, float2(-1, -0.56), float2(1, -0.56)) - 0.25, sd, 0.05);
                    sd = opUnion(sd, sdSegment(sdPos, float2(-0.2, -0.062), float2(0.2, -0.062))) - thickness;
                    return endSimple(f.color, sd, f);
                }

                if (id == 66)//B
                {
                    float sd = opOnion(sdRoundedBox(opSymX(sdPos) - float2(0, 0.17), float2(0.2, 0.16), float4(0.18, 0, 0, 0)), thickness);
                    return endSimple(f.color, sd, f);
                }

                if (id == 67)//C
                {
                    float sd = opOnion(sdRoundedBox(sdPos, float2(0.22, 0.31), 0.2), 0.001);
                    sd = opSmoothSubtraction(sdSegment(sdPos, float2(0.1, 0), float2(1, 0)) - 0.22, sd, 0.05) - thickness;
                    return endSimple(f.color, sd, f);
                }

                if (id == 68)//D
                {
                    float sd = opOnion(sdRoundedBox(sdPos, float2(0.2, 0.32), float4(0.2, 0.2, 0, 0)), thickness);
                    return endSimple(f.color, sd, f);
                }

                if (id == 69)//E
                {
                    float sd = sdSegment(opSymX(sdPos), float2(-0.2, 0.3), float2(0.2, 0.3));
                    sd = opUnion(sd, sdSegment(sdPos, float2(-0.2, 0), float2(0.16, 0)));
                    sd = opUnion(sd, sdSegment(sdPos, float2(-0.2, 0.3), float2(-0.2, -0.3))) - thickness;
                    return endSimple(f.color, sd, f);
                }

                if (id == 70)//F
                {
                    float sd = sdSegment(sdPos, float2(-0.2, 0.3), float2(0.2, 0.3));
                    sd = opUnion(sd, sdSegment(sdPos, float2(-0.2, 0), float2(0.16, 0)));
                    sd = opUnion(sd, sdSegment(sdPos, float2(-0.2, 0.3), float2(-0.2, -0.3))) - thickness;
                    return endSimple(f.color, sd, f);
                }

                if (id == 71)//G
                {
                    float sd = opOnion(sdRoundedBox(sdPos, float2(0.22, 0.31), float4(0.2, 0.1, 0.2, 0.2)), 0.001);
                    sd = opSmoothSubtraction(sdSegment(sdPos, float2(0.1, 0.062), float2(1, 0.062)) - 0.13, sd, 0.05);
                    sd = opUnion(sd, sdSegment(sdPos, float2(0.05, -0.05), float2(0.22, -0.05))) - thickness;
                    return endSimple(f.color, sd, f);
                }

                if (id == 72)//H
                {
                    float sd = sdSegment(opSymY(sdPos), float2(0.21, 0.3), float2(0.21, -0.3));
                    sd = opUnion(sd, sdSegment(sdPos, float2(0.12, 0), float2(-0.12, 0))) - thickness;
                    return endSimple(f.color, sd, f);
                }

                if (id == 73)//I
                {
                    float sd = sdSegment(opSymX(sdPos), float2(-0.162, 0.3), float2(0.162, 0.3));
                    sd = opUnion(sd, sdSegment(sdPos, float2(0, 0.3), float2(0, -0.3))) - thickness;
                    return endSimple(f.color, sd, f);
                }

                if (id == 74)//J
                {
                    float sd = opOnion(sdRoundedBox(sdPos, float2(0.22, 0.31), 0.2), 0.001);
                    sd = opSmoothSubtraction(sdSegment(sdPos, float2(1, 0.26), float2(-1, 0.26)) - 0.4, sd, 0.05);
                    sd = opUnion(sd, sdSegment(sdPos, float2(0.22, 0.3), float2(0.22, -0.1))) - thickness;
                    return endSimple(f.color, sd, f);
                }

                if (id == 75)//K
                {
                    float sd = sdSegment(opSymX(sdPos), float2(-0.12, 0), float2(0.162, 0.3));
                    sd = opUnion(sd, sdSegment(sdPos, float2(-0.22, 0.3), float2(-0.22, -0.3))) - thickness;
                    return endSimple(f.color, sd, f);
                }

                if (id == 76)//L
                {
                    float sd = sdSegment(sdPos, float2(-0.2, -0.3), float2(0.2, -0.3));
                    sd = opUnion(sd, sdSegment(sdPos, float2(-0.2, 0.3), float2(-0.2, -0.3))) - thickness;
                    return endSimple(f.color, sd, f);
                }

                if (id == 77)//M
                {
                    sdPos = opSymY(sdPos);
                    float sd = sdSegment(sdPos, float2(0.21, 0.3), float2(0.21, -0.3));
                    sd = opUnion(sd, sdSegment(sdPos, float2(0, 0), float2(0.21, 0.3))) - thickness;
                    return endSimple(f.color, sd, f);
                }

                if (id == 78)//N
                {
                    float sd = sdSegment(opSymY(sdPos), float2(0.21, 0.3), float2(0.21, -0.3));
                    sd = opUnion(sd, sdSegment(sdPos, float2(0.21, -0.3), float2(-0.21, 0.3))) - thickness;
                    return endSimple(f.color, sd, f);
                }

                if (id == 79)//O
                {
                    float sd = opOnion(sdRoundedBox(sdPos, float2(0.22, 0.31), 0.2), thickness);
                    return endSimple(f.color, sd, f);
                }

                if (id == 80)//P
                {
                    float sd = opOnion(sdRoundedBox(sdPos - float2(0, 0.15), float2(0.2, 0.16), float4(0.16, 0.16, 0, 0)), thickness);
                    sd = opUnion(sd, sdSegment(sdPos, float2(-0.2, 0.3), float2(-0.2, -0.3)) - thickness);
                    return endSimple(f.color, sd, f);
                }

                if (id == 81)//Q
                {
                    float sd = opOnion(sdRoundedBox(sdPos - float2(0, 0.05), float2(0.22, 0.28), 0.2), 0.001);
                    sd = opUnion(sd, sdSegment(sdPos, float2(0.05, -0.1), float2(0.2, -0.3))) - thickness;
                    return endSimple(f.color, sd, f);
                }

                if (id == 82)//R
                {
                    float sd = opOnion(sdRoundedBox(sdPos - float2(0, 0.15), float2(0.2, 0.16), float4(0.16, 0.16, 0, 0)), thickness);
                    sd = opUnion(sd, sdSegment(sdPos, float2(-0.2, 0.3), float2(-0.2, -0.3)) - thickness);
                    sd = opUnion(sd, sdSegment(sdPos, float2(0, -0.05), float2(0.2, -0.3)) - thickness);
                    return endSimple(f.color, sd, f);
                }

                if (id == 83)//S
                {
                    sdPos.y *= 1.2;
                    float sd = sdJoint2DSphere(rotate(sdPos, -1.62), 0.9, 2.3, thickness);
                    sd = opUnion(sd, sdJoint2DSphere(rotate(sdPos, 1.62), 0.9, 2.3, thickness));
                    return endSimple(f.color, sd, f);
                }

                if (id == 84)//T
                {
                    float sd = sdSegment(sdPos, float2(-0.22, 0.28), float2(0.22, 0.28));
                    sd = opUnion(sdSegment(sdPos, float2(0, 0.28), float2(0, -0.3)), sd) - thickness;
                    return endSimple(f.color, sd, f);
                }

                if (id == 85)//U
                {
                    float sd = opOnion(sdRoundedBox(sdPos - float2(0, 0.2), float2(0.22, 0.51), 0.2), 0.001);
                    sd = opSmoothSubtraction(sdSegment(sdPos, float2(-1, 0.53), float2(1, 0.53)) - 0.25, sd, 0.05) - thickness;
                    return endSimple(f.color, sd, f);
                }

                if (id == 86)//V
                {
                    float sd = sdSegment(opSymY(sdPos), float2(0, -0.3), float2(0.22, 0.3)) - thickness;
                    return endSimple(f.color, sd, f);
                }

                if (id == 87)//W
                {
                    sdPos = opSymY(sdPos);
                    float sd = sdSegment(sdPos, float2(0.21, 0.3), float2(0.21, -0.3));
                    sd = opUnion(sd, sdSegment(sdPos, float2(0, 0), float2(0.21, -0.3))) - thickness;
                    return endSimple(f.color, sd, f);
                }

                if (id == 88)//X
                {
                    float sd = sdSegment(abs(sdPos), 0, float2(0.22, 0.3)) - thickness;
                    return endSimple(f.color, sd, f);
                }

                if (id == 89)//Y
                {
                    float sd = sdSegment(opSymY(sdPos), 0, float2(0.22, 0.3));
                    sd = opUnion(sd, sdSegment(sdPos, 0, float2(0, -0.3))) - thickness;
                    return endSimple(f.color, sd, f);
                }

                if (id == 90)//Z
                {
                    float sd = sdSegment(opSymX(sdPos), float2(-0.2, 0.3), float2(0.2, 0.3));
                    sd = opUnion(sd, sdSegment(sdPos, float2(0.2, 0.3), float2(-0.2, -0.3))) - thickness;
                    return endSimple(f.color, sd, f);
                }

                return f.color;
            }
            ENDHLSL
        }
    }
}
