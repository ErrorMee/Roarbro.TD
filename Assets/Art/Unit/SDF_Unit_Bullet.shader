Shader "SDF/Unit/Bullet"
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

            float4 end(v2f f, float sketch)
            {
                sketch = opUnion(sketch, (0.48 - length(f.uv.xy)) * 64);
                f.color = paintIn(f.color, -sketch - 2);
                return endSimple(f.color, sketch);
            }

            half4 frag(v2f f) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(f);
                
                int id = UNITY_ACCESS_INSTANCED_PROP(Props, _Index);

                float2 sdPos = f.uv.xy; float2 symPos = opSymY(f.uv.xy);
                float sharp = min(_ScreenParams.x, _ScreenParams.y) * 0.1;
                
                if (id == 0)
                {
                    float sketch = sdCircle(sdPos, 0.48) * sharp;
                    return end(f, sketch);
                }

                if (id == 1)
                {
                    float sketch = sdCircle(sdPos, 0.48) * sharp;
                    return end(f, sketch);
                }
                
                if (id == 2)
                {
                    float sketch = sdMossEgg(float2(sdPos.x, -sdPos.y + 0.11) * 2.62) * sharp * 0.4;
                    return end(f, sketch);
                }
                
                if (id == 3)
                {
                    float _Radius1 = 0.35;
                    float _Radius2 = 0.1;
                    float h = 0.95 - (_Radius1 + _Radius2);

                    sdPos = float2(sdPos.x, -sdPos.y) - float2(0, _Radius1 - 0.48);
                    float sketch = sdUnevenCapsule(sdPos, _Radius1, _Radius2, h) * sharp;

                    return end(f, sketch);
                }

                if (id == 4)
                {
                    float sketch = sdCircle(sdPos, 0.48) * sharp;
                    return end(f, sketch);
                }

                if (id == 5)
                {
                    float sketch = sdCircle(sdPos, 0.48) * sharp;
                    return end(f, sketch);
                }

                return f.color;
            }
            ENDHLSL
        }
    }
}