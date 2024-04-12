Shader "ColorBlit"
{

    Properties
    {
        [NoScaleOffset] _MainTex("_MainTex", 2D) = "white" {}
        _BaseColor("MultiColor", Color) = (1, 1, 1, 1)
        _AddColor("DotColor", Color) = (1, 1, 1, 1)
        [Toggle(USEDot)] _Inverse("USEDot", float) = 0
        _Progress("_Progress", Range(0, 1)) = 0
    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" "Queue" = "Geometry" "IgnoreProjector" = "True" "RenderPipeline" = "UniversalPipeline"}

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
                    float2 texcoord  : TEXCOORD0;
                };

                sampler2D _MainTex;
                float4 _BaseColor;
                float4 _AddColor;
                float _Inverse;
                float _Progress;

                v2f vert(appdata v)
                {
                    v2f f;
                    VertexPositionInputs positionInputs = GetVertexPositionInputs(v.positionOS.xyz);
                    f.positionCS = positionInputs.positionCS;
                    f.texcoord = v.texcoord;
                    return f;
                }

                half4 frag(v2f f) : SV_Target
                {
                    float4 color = tex2D(_MainTex, f.texcoord);

                    float3 multiColor = color.rgb * _BaseColor.rgb;

                    float3 dotColor = dot(color.rgb, _AddColor.rgb);//gray: dot(color.rgb, float3(0.2126, 0.7152, 0.0722))

                    float useDot = _Inverse;

                    float3 targetColor = (1 - useDot) * multiColor + useDot * dotColor;

                    color.rgb = lerp(color.rgb, targetColor, _Progress);
                    return color;
                }

                ENDHLSL
            }
        }
}