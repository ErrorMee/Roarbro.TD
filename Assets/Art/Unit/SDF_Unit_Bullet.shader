Shader "SDF/Unit/Bullet"
{
    Properties
    {
        _BaseColor("_BaseColor", Color) = (1, 1, 1, 1)
        _AddColor("_AddColor", Color) = (1, 1, 1, 1)
        [IntRange] _Index("_Index", Range(0, 5)) = 0
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
            #include "../SDFLib/Noise.hlsl"

            float4 end(v2f f, float sketch)
            {
                f.color = paintIn(f.color, -sketch - 2.62, f.color * 0.1);
                return endSimple(f.color, sketch);
            }

            float inCirc(float t) { return 1 - sqrt(1 - (t * t)); }
            float4 sdGlowColor(float sd, float4 top, float sharp = 16, float brightness = 2)
            {
                float alpha = saturate(-(sd - 1.0 / sharp) * sharp);
                alpha = inCirc(alpha);
                top.a *= alpha;
                top.rgb *= max(1 - sd * brightness, 1);
                return top;
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
                    float rotRadian = fmod(_Time.y, TWO_PI) * 4;
                    sdPos.xy = rotate(sdPos.xy, rotRadian);
                    sdPos = rotate(sdPos.xy, -PI * length(sdPos.xy));
                    float sketch = opOnion(sdStar(sdPos, 0.35, 6, 3.2), 0.01) - 0.1;
                    return end(f, sketch * sharp);
                }

                if (id == 5)
                {
                    f.uv.y = -f.uv.y;
                    float2 sdPos = f.uv.xy;
                    float sag = max(f.uv.y, 0);
                    sdPos.y += sag;
                    float sd = sdCircle(sdPos, 0.4);

                    float noiseV = noiseWave(f.uv.xy * 6 - float2(0, _Time.y * 4)) * 0.5 + 0.5;//0-1
                    float len = length(f.uv.xy);
                    float dist = noiseV * max(0.5 - len, 0);
                    dist *= 1 + sag * 24;
                    sd -= dist;
                    half4 addColor = half4(f.color.rgb * 2, 1);
                    f.color = lerp(f.color, half4(addColor.rgb, f.color.a), smoothstep(0.5, 0.55, dist));
                    half4 color = sdGlowColor(sd, f.color, 12 * (1 - sag));
                    return color;
                }

                return f.color;
            }
            ENDHLSL
        }
    }
}