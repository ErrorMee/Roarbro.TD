//https://github.com/QianMo/X-PostProcessing-Library/blob/master/Assets/X-PostProcessing/Effects/BokehBlur/Shader/BokehBlur.shader
Shader "Roarbro/Blur/Bokeh"
{
    Properties
    {
        [NoScaleOffset] _MainTex("_MainTex", 2D) = "white" {}
        [IntRange] _Iteration("_Iteration", Range(8, 160)) = 32
        [IntRange] _IterationBlank("_IterationBlank", Range(0, 96)) = 32
        _Radius("_Radius", Range(0.1, 10)) = 1
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
                    float4 texcoord  : TEXCOORD0;
                };

                sampler2D _MainTex;
                int _Iteration; int _IterationBlank;
                float _Radius;

                v2f vert(appdata v)
                {
                    v2f f;
                    VertexPositionInputs positionInputs = GetVertexPositionInputs(v.positionOS.xyz);
                    f.positionCS = positionInputs.positionCS;
                    f.texcoord.xy = v.texcoord;
                    f.texcoord.zw = _Radius * 0.0005 * (_ScreenParams.yx / _ScreenParams.y);
                    return f;
                }

                half4 frag(v2f f) : SV_Target
                {
                    float2x2 rot1radian = float2x2(0.5403023059, -0.8414709848, 0.8414709848, 0.5403023059);
                    float4 accumulator = 0.0;
                    float4 divisor = 0.0;

                    float dis = 1.0;
                    float2 rot = float2(0.0, 1);
                    float2 pixelSize = f.texcoord.zw;

                    int iteration = _Iteration;

                    for (int j = 0; j < _IterationBlank; j++)
                    {
                        dis += 1.0 / dis;
                        rot = mul(rot1radian, rot);
                    }

                    for (int j = _IterationBlank; j < _Iteration; j++)
                    {
                        dis += 1.0 / dis;
                        rot = mul(rot1radian, rot);
                        float2 offsetUV = (dis - 1.0) * rot * pixelSize;

                        float2 uv = float2(f.texcoord.xy + offsetUV);
                        float4 bokeh = tex2D(_MainTex, uv);
                        accumulator += bokeh * bokeh;
                        divisor += bokeh;
                    }
                    float4 rst = accumulator / divisor;
                    //rst.rgb *= 0.618;
                    return rst;
                }

                ENDHLSL
            }
        }
}