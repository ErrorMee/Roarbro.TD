
#if !defined(MY_Blend)
#define MY_Blend

float4 blendColor(float4 top, float4 bot)
{
    float3 color = (top.rgb * top.a) + (bot.rgb * (1 - top.a));
    float alpha = top.a + bot.a * (1 - top.a);
    return float4(color, alpha);
}

#endif