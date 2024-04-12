Shader "SDF/UI/Roarbro"
{
    Properties
    {
        [HideInInspector] [NoScaleOffset] _MainTex("_MainTex", 2D) = "white" {}
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
            #include "../../SDFLib/SDFImage.hlsl"

            half4 frag(v2f f) : SV_Target
            {
                float2 sdPos = f.uv.xy;
                float radius = 0.4; float roundness = 0.05;
                float childRadius = radius * 0.41421356;//1 / (1 + sqrt(2))

                float sdTail = opIntersection(sdCircle(sdPos, radius), sdCircle(sdPos - float2(radius + childRadius, 0), radius));
                sdTail = opSubtraction(sdCircle(sdPos - childRadius, childRadius), sdTail);
                float sdHead = sdCircle(sdPos - float2(childRadius, -childRadius), childRadius);

                float sd = opUnion(sdTail, sdHead);
                sd = opSubtraction(sdCircle(sdPos - float2(childRadius, -childRadius), childRadius * 0.5), sd);
                sd = opSubtraction(sd, sdCircle(sdPos, radius + roundness));

                return endWithOutGlow(f.color, sd, f);

            }
            ENDHLSL
        }
    }
}
