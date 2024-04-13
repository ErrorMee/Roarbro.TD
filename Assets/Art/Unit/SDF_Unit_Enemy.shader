Shader "SDF/Unit/Enemy"
{
    Properties
    {
        _BaseColor("_BaseColor", Color) = (1, 1, 1, 1)
        _AddColor("_AddColor", Color) = (1, 1, 1, 1)
        [IntRange] _Index("_Index", Range(0, 8)) = 0
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

            half4 frag(v2f f) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(f);
                int id = UNITY_ACCESS_INSTANCED_PROP(Props, _Index);
                float4 addColor = UNITY_ACCESS_INSTANCED_PROP(Props, _AddColor);
                float2 sdPos = f.uv.xy;
                float sharp = min(_ScreenParams.x, _ScreenParams.y) * 0.2;

                float animatX = (abs(frac(_Time.y * 4) - 0.5) * 2 - 0.5);
                float2 symUVRot = opSymY(sdPos);

                float sd1 = sdRoundedBox(sdPos + float2(0, 0.1), float2(0.225, 0.36), 0.2);
                float sd2 = sdTriangle(symUVRot, float2(0.05, 0.2),
                    float2(0.125 + animatX * 0.05, 0.38),
                    float2(0.17, 0.1)) - 0.025;

                float sd3 = sdTriangle(symUVRot, float2(0.14, 0.15),
                    float2(0.35 + animatX * 0.1, animatX * 0.1),
                    float2(0.14, -0.35)) - 0.05;

                float sd4 = sdCircle(symUVRot + float2(-0.05 - animatX * 0.003, -0.1), 0.03);
                f.color = paintIn(f.color, sd4 * sharp, addColor);

                float sd = opUnion(opUnion(sd1, sd2), sd3);
                //sd = opUnion(sd, opOnion(sdCircle(sdPos, 0.5), 0.01));
                return endSimple(f.color, sd * sharp);
            }
            ENDHLSL
        }
    }
}