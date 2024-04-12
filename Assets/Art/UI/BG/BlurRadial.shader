//https://www.shadertoy.com/view/lsSXR3
Shader "Roarbro/Blur/Radial"
{
    Properties
    {
        [NoScaleOffset] _MainTex("_MainTex", 2D) = "white" {}
        _Iteration("_Iteration", Range(8, 32)) = 32
        _Radius("x:start,y:end,z:speedStart,w:speedEnd", Vector) = (0.2, 0.5, 0.01, 0.01)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" "IgnoreProjector"="True" "RenderPipeline"="UniversalPipeline"}

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 positionOS : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 positionCS : SV_POSITION;
                float4 texcoord  : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _Radius;
            int _Iteration;

            v2f vert(appdata v)
            {
                v2f f;
                VertexPositionInputs positionInputs = GetVertexPositionInputs(v.positionOS.xyz);
                f.positionCS = positionInputs.positionCS;
                f.texcoord.xy = v.texcoord;
                f.texcoord.zw = f.texcoord.xy - 0.5;
                return f;
            }

            half4 frag(v2f f) : SV_Target
            {
                float2 offset = float2(0.5, 0.5) - f.texcoord.xy;
                
                float dis = length(offset);

                int iteration = step(_Radius.x, dis) * _Iteration + 1;
                
                float2 blurVector = offset * lerp(_Radius.z, _Radius.w, saturate((dis - _Radius.x) / (_Radius.y - _Radius.x)));

                float4 acumulateColor = float4(0, 0, 0, 0);

                [unroll(32)]
                for (int j = 0; j < iteration; j++)
                {
                    float4 tempColor = tex2D(_MainTex, f.texcoord);
                    acumulateColor += tempColor;

                    tempColor.rgb = dot(tempColor.rgb, float3(0.222, 0.707, 0.071));
                    acumulateColor += tempColor;

                    f.texcoord.xy += blurVector;
                }
                float4 rst = acumulateColor / (iteration * 2);
                return rst;
            }

            ENDHLSL
        }
    }
}