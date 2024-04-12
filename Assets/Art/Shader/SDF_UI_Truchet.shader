Shader "SDF/UI/Truchet"
{
    Properties
    {
        [HideInInspector] [NoScaleOffset] _MainTex("_MainTex", 2D) = "white" {}
        _TileRadius("_TileRadius", Range(1, 1024)) = 32
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
            #include "../SDFLib/Noise.hlsl"

            float _TileRadius;

        float3 GetSdPos4(inout v2f f)
        {
            float2 sdPos = abs(fmod(f.os.zw + _TileRadius, _TileRadius * 2)) - _TileRadius;
            float2 tilePos = round(f.os.zw / (_TileRadius * 2));
            float noise = hash12(tilePos);
            float noiseIndex = floor(noise * 4);
            sdPos = rotate(sdPos, noiseIndex * HALF_PI);

            f.uv.z = 0.5;
            float rr = (1.0 / f.effect.w) * 0.05 * _TileRadius;
            f.effect.x = f.effect.x * rr;
            f.effect.y = f.effect.y * rr;

            return float3(sdPos.x, sdPos.y, noiseIndex);
        }

        float4 endColor(v2f f, float sd, float aa)
        {
            float4 color = endWithOnionOutLine(f.color, sd, f);
            float alpha = saturate(-sdCircle(f.uv.xy, 0.5) * aa);
            color.a *= alpha;
            return color;
        }

            half4 frag(v2f f) : SV_Target
            {
                int id = f.uv.w;
                float aa = f.uv.z;
                
                if (id == 0)
                {
                    float2 sdPos = GetSdPos4(f).xy;
                    float tileRadius2 = _TileRadius * 2;
                    float tileRadiusHalf = _TileRadius * 0.35;
                    float tileRadiusHH = _TileRadius * 0.25;
                    float sd = sdSegment(opSym45(sdPos), float2(-tileRadius2, -_TileRadius), float2(tileRadius2, tileRadius2 + _TileRadius));
                    sd = opUnion(sd, sdSegment(sdPos, float2(-tileRadiusHalf, tileRadiusHalf) - tileRadiusHH,
                        float2(tileRadiusHalf, -tileRadiusHalf) - tileRadiusHH));
                    return endColor(f, sd, aa);
                }

                if (id == 1)
                {
                    _TileRadius *= 1.3;
                    float3 sdPos = GetSdPos4(f);

                    float2 a = float2(-_TileRadius, 0); float2 a1 = float2(-_TileRadius * (0.75 + (sdPos.z - 1) * 0.04), 0);
                    float2 b = float2(0, _TileRadius); float2 b1 = float2(0, _TileRadius * 0.85);
                    float2 c = float2(_TileRadius, 0); float2 c1 = float2(_TileRadius * 0.9, 0);
                    float2 d = float2(0, -_TileRadius); float2 d1 = float2(0, -_TileRadius * 0.95);

                    float2 sdPos2 = GetSdPos4(f).xy;
                    float sd = sdSegment(sdPos2, a, a1);
                    sd = opSmoothUnion(sd, sdSegment(sdPos2, b, b1), 4);
                    sd = opSmoothUnion(sd, sdSegment(sdPos2, c, c1), 4);
                    sd = opSmoothUnion(sd, sdSegment(sdPos2, d, d1), 4);

                    sd = opSmoothUnion(sd, sdSegment(sdPos2, a1, b1), 4);
                    sd = opSmoothUnion(sd, sdSegment(sdPos2, b1, c1), 4);
                    sd = opSmoothUnion(sd, sdSegment(sdPos2, c1, d1), 4);
                    sd = opSmoothUnion(sd, sdSegment(sdPos2, a1, d1), 4);
                    return endColor(f, sd, aa);
                }

                if (id == 2)
                {
                    float2 sdPos = GetSdPos4(f).xy;
                    float sd = sdCircle(sdPos - _TileRadius, _TileRadius);
                    sd = opSmoothUnion(sd, sdCircle(sdPos - float2(-_TileRadius, _TileRadius), _TileRadius), _TileRadius * 0.5);
                    sd = opUnion(sd, sdCircle(sdPos - float2(0, -_TileRadius), 0));
                    return endColor(f, sd, aa);
                }

                if (id == 3)
                {
                    float2 sdPos = GetSdPos4(f).xy;
                    float sd = sdCircle(sdPos - _TileRadius, _TileRadius);
                    sd = opUnion(sd, sdCircle(sdPos + _TileRadius, _TileRadius));
                    return endColor(f, sd, aa);
                }

                return f.color;
            }
            ENDHLSL
        }
    }
}
