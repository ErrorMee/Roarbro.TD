Shader "SDF/UI/Stage"
{
    Properties
    {
        [HideInInspector] [NoScaleOffset] _MainTex("_MainTex", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline"}
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #include "../SDFLib/SDFImage.hlsl"

            half4 frag(v2f f) : SV_Target
            {
                float2 sdPos = f.uv.xy;
                float id = f.uv.w;
                float radius = 0.4; float roundness = 0.1;
                float sd = sdCircle(sdPos, radius);
                return endWithOnionOutGlow(f.color, sd, f);
            }
            ENDHLSL
        }
    }
}
