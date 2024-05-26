Shader "SDF/Unit/Ball"
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

            float4 end(float4 color, float sketch, float pattern, float4 baseColor)
            {
                float4 patternColor = baseColor; patternColor.rgb *= 0.5;
                color = paintIn(color, pattern, patternColor);

                return endSimple(color, sketch);
            }

            half4 frag(v2f f) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(f);
                
                int id = UNITY_ACCESS_INSTANCED_PROP(Props, _Index);

                float4 baseColor = f.color;
                float4 patternColor = baseColor; patternColor.rgb *= 0.5;
                float2 sdPos = f.uv.xy; float2 symPos = opSymY(f.uv.xy);
                float sharp = min(_ScreenParams.x, _ScreenParams.y) * 0.1;
                float sketch = sdCircle(sdPos, 0.48) * sharp;
                
                float head = sdCircle(symPos - float2(0, 0.4), 0.2);
                float pattern = opSubtraction(sdCircle(symPos - float2(0.08, 0.36), 0.05), head);
                if (id == 0)
                {
                    float rotRadian = fmod(_Time.y, TWO_PI) * 2;
                    float2 sdPosRot = rotate(sdPos, rotRadian);
                    pattern = sdStar5(sdPosRot, 0.22, 0.62) - 0.05;
                    return end(f.color, sketch, pattern * sharp, baseColor);
                }

                if (id == 1)
                {
                    pattern = opUnion(pattern, sdCircle(symPos - float2(0, 0.05), 0.1));
                    pattern = opUnion(pattern, sdCircle(symPos - float2(0, -0.42), 0.13));
                    pattern = opUnion(pattern, sdCircle(symPos - float2(0.2, -0.16), 0.11));
                    pattern = opUnion(pattern, sdCircle(symPos - float2(0.4, 0.15), 0.12));
                    return end(f.color, sketch, pattern * sharp, baseColor);
                }
                
                if (id == 2)
                {
                    pattern = opUnion(pattern, opOnion(sdCircle(symPos - float2(0, 0.52), 0.45), 0.05));
                    pattern = opUnion(pattern, opOnion(sdCircle(symPos - float2(0, 0.52), 0.65), 0.05));
                    pattern = opUnion(pattern, opOnion(sdCircle(symPos - float2(0, 0.52), 0.85), 0.05));
                    return end(f.color, sketch, pattern * sharp, baseColor);
                }
                
                if (id == 3)
                {
                    pattern = opUnion(pattern, opOnion(sdHexagon(symPos - float2(0, 0), 0.16), 0.04));
                    pattern = opUnion(pattern, opOnion(sdHexagon(symPos - float2(0, -0.42), 0.16), 0.04));
                    symPos = abs(f.uv.xy);
                    pattern = opUnion(pattern, opOnion(sdHexagon(symPos - float2(0.38, 0.21), 0.16), 0.04));
                    return end(f.color, sketch, pattern * sharp, baseColor);
                }

                if (id == 4)
                {
                    float pre = sdCircle(symPos - float2(0.4, 0.4), 0.2);
                    float pre1 = opUnion(head + 0.01, pre);
                    pattern = opUnion(pattern, opOnion(pre, 0.04));

                    float pre2 = opUnion(sdCircle(symPos - float2(0.2, 0.2), 0.2), pre1);
                    pre2 = opUnion(pre2, sdCircle(symPos - float2(0.6, 0.2), 0.2));
                    pattern = opUnion(pattern, opSubtraction(pre1, opOnion(pre2, 0.04)));

                    float pre3 = opUnion(sdCircle(symPos - float2(0, 0), 0.2), sdCircle(symPos - float2(0.4, 0), 0.2));
                    pattern = opUnion(pattern, opSubtraction(pre2, opOnion(pre3, 0.04)));
                    
                    float pre4 = opUnion(sdCircle(symPos - float2(0.2, -0.2), 0.2), sdCircle(symPos - float2(0.6, -0.2), 0.2));
                    pattern = opUnion(pattern, opSubtraction(pre3, opOnion(pre4, 0.04)));

                    return end(f.color, sketch, pattern * sharp, baseColor);
                }

                if (id == 5)
                {
                    pattern = opUnion(pattern, opSubtraction(head + 0.01, opOnion(sdCircle(symPos, 0.35), 0.04)));
                    pattern = opUnion(pattern, opOnion(sdCircle(symPos, 0.2), 0.04));
                    pattern = opUnion(pattern, sdCircle(symPos, 0.08));
                    return end(f.color, sketch, pattern * sharp, baseColor);
                }

                return f.color;
            }
            ENDHLSL
        }
    }
}