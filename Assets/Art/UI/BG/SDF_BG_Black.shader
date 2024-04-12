Shader "Roarbro/SDF/BG/Black"
{
    Properties
    {
        _BaseColor("_EdgeColor", Color) = (0.1137255, 0.1163398, 0.1294117, 1)
        _AddColor("_CenterColor", Color) = (0.2549019, 0.2607843, 0.2901961, 1)
        _CenterPercent("_CenterPercent", Range(0, 1)) = 0.01
    }
    SubShader
    {
        Tags { "RenderType" = "Background" "Queue" = "Background" "RenderPipeline" = "UniversalPipeline"}

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "../../SDFLib/SDF2D.hlsl"
            #include "../../SDFLib/Noise.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            float4 _BaseColor;
            float4 _AddColor; 
            float _CenterPercent;

            struct appdata
            {
                float4 os       : POSITION;
                float2 uv       : TEXCOORD0;
            };

            struct v2f
            {
                float4 cs       : SV_POSITION;
                float4 uv       : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f f;
                VertexPositionInputs vertexInput;
                vertexInput.positionWS = TransformObjectToWorld(v.os.xyz);
                vertexInput.positionCS = TransformWorldToHClip(vertexInput.positionWS);
                f.cs = vertexInput.positionCS;

                f.uv.xy = v.uv.xy - 0.5;
                f.uv.xy *= _ScreenParams.xy / _ScreenParams.y;
                f.uv.zw = abs(f.uv.xy);
                return f;
            }

            half4 frag(v2f f) : SV_Target
            {
                float unSolid = 0.5 - _CenterPercent * 0.5;
                float sd = sdBox(f.uv.xy, f.uv.zw - unSolid);
                float n = hash12(f.cs.xy); sd += (n - 0.5) * 0.1;

                float gradient = clamp(sd / unSolid * 1.1, 0.0, 1.0);
                float4 rst = 1;
                rst.rgb = _AddColor.rgb * (1 - gradient) + _BaseColor.rgb * gradient;
                return rst;
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}