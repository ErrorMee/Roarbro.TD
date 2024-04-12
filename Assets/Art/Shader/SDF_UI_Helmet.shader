Shader "SDF/UI/Helmet"
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
                float id = f.uv.w;
                float radius = 0.35; float edgeSafe = 0.062; float yOffset = (0.5 - radius) - edgeSafe;
                float2 sdPos = f.uv.xy + float2(0, yOffset);
                float head = sdQuadraticCircle(sdPos, radius);

                float hairRadius = (radius + (0.5 - sdPos.y)) * 0.1;
                float hairTop = (0.5 - edgeSafe) + yOffset * 0.5;
                float hairMid = (hairTop + radius) * 0.5;
                float hairW = radius * 0.262;
                float hair = 1;

                if (id == 0)
                {
                    return endWithOutGlow(f.color, head, f);
                }

                if (id == 1)
                {
                    hair = sdBezier(sdPos, float2(-hairW, hairTop), float2(0, hairMid), float2(hairW, hairTop));
                    hair = opSmoothUnion(hair, sdSegment(sdPos, float2(0, radius), float2(0, hairMid)), 0.01) - hairRadius;

                    return endWithOutGlow(f.color, opSmoothUnion(head, hair, 0.01), f);
                }
                if (id == 2)
                {
                    float s; float c; sincos(HALF_PI, s, c);
                    hair = sdArc(rotate(opSymY(sdPos) - float2(0.1, 0.4), -HALF_PI), float2(s, c), 0.1, hairRadius - 0.01);
                    hair = opUnion(hair, sdArc(rotate(opSymY(sdPos) - float2(0.1, 0.45), HALF_PI), float2(s, c), 0.05, hairRadius - 0.01));
                    return endWithOutGlow(f.color, opUnion(head, hair), f);
                }
                if (id == 3)
                {
                    hair = sdTriangle(sdPos, float2(0, 0.46), float2(-0.12, 0.3), float2(0.12, 0.3));
                    hair = opUnion(hair, sdTriangle(opSymY(sdPos), float2(0.12, 0.4), float2(-0.12, 0.3), float2(0.14, 0.3)));
                    hair -= 0.05;
                    return endWithOutGlow(f.color, opUnion(head, hair), f);
                }
                if (id == 4)
                {
                    hair = sdSegment(opSymY(sdPos), float2(0.01, radius), float2(0, hairTop)) - hairRadius;
                    return endWithOutGlow(f.color, opUnion(head, hair), f);
                }
                if (id == 5)
                {
                    hair = sdBezier(opSymY(sdPos), float2(0.29, 0.46), float2(0.27, 0.37), float2(0.12, 0.29)) - hairRadius;
                    hair = opUnion(hair, sdSegment(opSymY(sdPos), float2(0.2, 0.35), float2(0.3, 0.35)) - hairRadius + 0.01);
                    return endWithOutGlow(f.color, opUnion(head, hair), f);
                }
                if (id == 6)
                {
                    hair = sdBezier(opSymY(sdPos), float2(0.32, 0.48), float2(0.33, 0.32), float2(0.12, 0.27)) - (0.02 + (0.5 - sdPos.y) * 0.2);
                    return endWithOutGlow(f.color, opUnion(head, hair), f);
                }

                if (id == 7)
                {
                    hair = sdCircle(opSymY(sdPos) - float2(0.25, 0.34), 0.06 + hairRadius);
                    return endWithOutGlow(f.color, opUnion(head, hair), f);
                }
                if (id == 8)
                {
                    hair = sdBezier(sdPos, float2(0.1, 0.3), float2(0.1, 0.42), float2(0.16, 0.43)) - 0.08;
                    hair = opUnion(hair, sdBezier(sdPos, float2(-0.1, 0.3), float2(-0.1, 0.42), float2(-0.05, 0.43)) - 0.08);
                    return endWithOutGlow(f.color, opUnion(head, hair), f);
                }
                if (id == 9)
                {
                    hair = sdCircle(sdPos - float2(0.2, 0.3), 0.13);
                    hair = opUnion(hair, sdCircle(sdPos - float2(0.28, 0.4), 0.08));
                    hair = opUnion(hair, sdCircle(sdPos - float2(0.33, 0.46), 0.04));
                    return endWithOutGlow(f.color, opUnion(head, hair), f);
                }
                if (id == 10)
                {
                    hair = sdTriangle(opSymY(sdPos), float2(0.03, 0.15), float2(0.25, 0.38), float2(0.28, 0.05)) - 0.05;
                    return endWithOutGlow(f.color, opUnion(head, hair), f);
                }
                

                if (id == 32)
                {
                    radius = 0.21;
                    float glass = sdBezier(sdPos, float2(-radius, 0), float2(0, radius * 0.262), float2(radius, 0)) - radius;
                    return endWithOutGlow(f.color, glass, f);
                }

                return endWithOutGlow(f.color, head, f);
            }
            ENDHLSL
        }
    }
}
