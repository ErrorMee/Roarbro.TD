Shader "Roarbro/Blur/Gaussian"
{
    Properties
    {
        [NoScaleOffset] _MainTex("_MainTex", 2D) = "white" {}
        
        _BlurRadius("_BlurRadius", Range(1, 16)) = 4
        _BlurCircleSegment("_BlurCircleSegment", Range(6, 24)) = 12
        _BlurRadiusSegment("_BlurRadiusSegment", Range(2, 5)) = 3
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
                float2 texcoord  : TEXCOORD0;
            };

            sampler2D _MainTex;
            float _BlurCircleSegment;
            float _BlurRadiusSegment;
            float _BlurRadius;

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
                half4 Color = tex2D(_MainTex, f.texcoord);

                float2 radius = _BlurRadius / _ScreenParams.xy;

                int blurCircleSegment = _BlurCircleSegment;
                int blurRadiusSegment = _BlurRadiusSegment;

                float circleSegment = TWO_PI / blurCircleSegment;
                float radiusSegment = 1.0 / blurRadiusSegment;
                float radiusSegmentStart = radiusSegment;
                
                for(float d = circleSegment; d <= TWO_PI; d += circleSegment)
                {
                    float s; float c; sincos(d, s, c);
                    float2 normalRadius = float2(c, s);

		            for(float i = radiusSegmentStart; i <= 1; i += radiusSegment)
                    {
			            Color += tex2D(_MainTex, f.texcoord + normalRadius * radius * i);	
                    }
                }
                
                Color /= (blurRadiusSegment * blurCircleSegment);
                //Color.rgb *= 0.618;
                return Color;
            }

            ENDHLSL
        }
    }
}