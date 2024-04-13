Shader "SDF/Unit/Piece"
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

            float4 end(float4 color, float eye, float sketch, float pattern, float4 baseColor)
            {
                float4 patternColor = baseColor; patternColor.rgb *= 0.5;
                color = paintIn(color, pattern, patternColor);

                float4 eyeColor = baseColor; eyeColor.rgb *= 0.1;
                color = paintIn(color, eye, eyeColor);

                return endSimple(color, sketch);
            }

            half4 frag(v2f f) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(f);
                
                int id = UNITY_ACCESS_INSTANCED_PROP(Props, _Index);

                float4 baseColor = f.color;
                float4 patternColor = baseColor; patternColor.rgb *= 0.5;
                float2 sdPos = f.uv.xy; float2 sdPosSym = opSymY(f.uv.xy);
                float sharp = min(_ScreenParams.x, _ScreenParams.y) * 0.08;
                float sketch = sdCircle(sdPos, 0.48) * sharp;
                float eye = sdCircle(sdPosSym - float2(0.08, 0.36), 0.066);
                eye = opUnion(eye, -sdCircle(sdPos, 0.43)) * sharp;

                float pattern = sdCircle(sdPosSym - float2(0, 0.4), 0.2);


                if (id == 0)
                {
                    pattern = opUnion(pattern, sdCircle(sdPosSym - float2(0, 0.05), 0.1));
                    pattern = opUnion(pattern, sdCircle(sdPosSym - float2(0, -0.42), 0.13));
                    pattern = opUnion(pattern, sdCircle(sdPosSym - float2(0.2, -0.16), 0.11));
                    pattern = opUnion(pattern, sdCircle(sdPosSym - float2(0.4, 0.15), 0.12));

                    return end(f.color, eye, sketch, pattern * sharp, baseColor);
                }
                
                if (id == 1)
                {
                    pattern = opUnion(pattern, opOnion(sdCircle(sdPosSym - float2(0, 0.5), 0.5), 0.062));
                    pattern = opUnion(pattern, opOnion(sdCircle(sdPosSym - float2(0, 0.5), 0.75), 0.062));
                    return end(f.color, eye, sketch, pattern * sharp, baseColor);
                }
                
                if (id == 2)
                {
                    pattern = opUnion(pattern, opOnion(sdHexagon(sdPosSym - float2(0, 0), 0.14), 0.04));
                    pattern = opUnion(pattern, opOnion(sdHexagon(sdPosSym - float2(0, -0.4), 0.15), 0.04));
                    sdPosSym = abs(f.uv.xy);
                    pattern = opUnion(pattern, opOnion(sdHexagon(sdPosSym - float2(0.32, 0.2), 0.14), 0.04));
                    return end(f.color, eye, sketch, pattern * sharp, baseColor);
                }

                return f.color;
            }
            ENDHLSL
        }
    }
}