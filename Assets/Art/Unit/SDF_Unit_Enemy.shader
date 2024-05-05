Shader "SDF/Unit/Enemy"
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

            float4 end(float4 color, float sd)
            {
                color = endSimple(color, sd); 
                color.rgba = lerp(color.rgba, float4(0.1, 0.1, 0.15, color.a * 0.33), saturate(sd + 7));
                return color;
            }

            half4 frag(v2f f) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(f);
                int id = UNITY_ACCESS_INSTANCED_PROP(Props, _Index);
                float4 addColor = UNITY_ACCESS_INSTANCED_PROP(Props, _AddColor);
                float2 sdPos = f.uv.xy;
                float sharp = min(_ScreenParams.x, _ScreenParams.y) * 0.2;
                float animatX = (abs(frac(_Time.y * 2) - 0.5) * 2 - 0.5);
                //animatX = 0.5;
                float2 sdPosSymY = opSymY(sdPos);

                if (id == 0)
                {
                    return 0;
                }

                if (id == 1)
                {
                    float sd1 = sdTriangle(sdPosSymY, float2(0, -0.2),
                        float2(0.33 + animatX * 0.02, -animatX * 0.02),
                        float2(0.01, 0.38)) - 0.1;

                    float sd = sdBezier(sdPosSymY, float2(0.125, -0.15), float2(0.175, -0.3),
                        float2(0.23 + 0.08 * animatX, -0.32 + 0.08 * animatX));
                    float sd3 = sdBezier(sdPosSymY, float2(0.025, -0.15), float2(0.04, -0.325),
                        float2(0.07 + 0.06 * animatX, -0.4));

                    sd = opUnion(sd, sd3) - 0.09 - sdPosSymY.y * 0.05;
                    sd = opUnion(sd1, sd);

                    float sd4 = sdCircle(sdPosSymY + float2(-0.07, -0.2), 0.04);
                    f.color = paintIn(f.color, sd4 * sharp, addColor);

                    return end(f.color, sd * sharp);
                }

                if (id == 2)
                {
                    float sd1 = sdCircle(sdPosSymY + float2(0, -0.1), 0.38 + animatX * 0.01);
                    float sd = sdBezier(sdPosSymY, float2(0.125, -0.15), float2(0.175, -0.3),
                        float2(0.23 + 0.08 * animatX, -0.32 + 0.08 * animatX));
                    float sd3 = sdBezier(sdPosSymY, float2(0.025, -0.15), float2(0.04, -0.325),
                        float2(0.07 + 0.06 * animatX, -0.4));

                    sd = opUnion(sd, sd3) - 0.09 - sdPosSymY.y * 0.05;
                    sd = opUnion(sd1, sd);

                    float sd4 = sdBox(sdPosSymY + float2(-0.075, -0.2), float2(0.0156, 0.05)) - 0.02;
                    f.color = paintIn(f.color, sd4 * sharp, addColor);
                    return end(f.color, sd * sharp);
                }

                if (id == 3)
                {
                    float sd = sdCircle(sdPosSymY + float2(0, -0.162), 0.162);
                    float sd1 = sdTriangle(sdPosSymY, float2(0.05, -0.2),
                        float2(0.31 + animatX * 0.05, 0), float2(0.135, 0.25));

                    sd = opUnion(sd1, sd) - 0.15;
                    sd = opUnion(sdSegment(sdPos, float2(0, -0.25), float2(animatX * 0.1, -0.42 + abs(animatX * 0.05))) - 0.07, sd);

                    float sd4 = sdCircle(sdPosSymY + float2(-0.075, -0.2), 0.04);
                    f.color = paintIn(f.color, sd4 * sharp, addColor);
                    return end(f.color, sd * sharp);
                }

                return f.color;
            }
            ENDHLSL
        }
    }
}