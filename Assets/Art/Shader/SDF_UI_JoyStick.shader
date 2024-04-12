Shader "SDF/UI/JoyStick"
{
    Properties
    {
        [HideInInspector] [NoScaleOffset] _MainTex("_MainTex", 2D) = "white" {}
        _SizeA("_SizeA", Range(0, 0.5)) = 0.3
        _SizeB("_SizeB", Range(0, 0.5)) = 0.15
        _Progress("_Distance", Range(-1, 1)) = 0
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
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #include "../SDFLib/SDFImage.hlsl"

            float _SizeA; float _SizeB; float _Progress;

            half4 frag(v2f f) : SV_Target
            {
                float sdA = sdCircle(f.uv.xy, _SizeA);
                float sdB = sdCircle(f.uv.xy - float2(0, _Progress * (0.46 - _SizeB)), _SizeB);
                float sd = opSmoothUnion(sdA, sdB, _SizeB);
                return endWithOutGlow(f.color, sd, f);
                //return sdClipRect(endEffect(f.color, sd, f), f.os.xy, _ClipRect);
            }
            ENDHLSL
        }
    }
}