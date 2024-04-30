//https://iquilezles.org/articles/distfunctions/
#if !defined(MY_SDF_INCLUDED)
#define MY_SDF_INCLUDED

#include "Math.hlsl"

//----------alterations----------
float opRound(float sd, float roundness)
{
    return sd - roundness;
}

float opOnion(float sd, float r)//Onion Mid
{
    float hasOnion = AGreatB(r, 0.000001);
    return hasOnion * (abs(sd) - r) + (1 - hasOnion) * sd;
}

float opOnionIn(float sd, float r)//Onion In
{
    float hasOnion = AGreatB(r, 0.000001);
    return hasOnion * (abs(sd + r) - r) + (1 - hasOnion) * sd;
}

//----------combinations----------
float opUnion(float d1, float d2) { return min(d1, d2); }
float opSmoothUnion(float d1, float d2, float k) {
    float h = clamp(0.5 + 0.5 * (d2 - d1) / k, 0.0, 1.0);
    return lerp(d2, d1, h) - k * h * (1.0 - h);
}

float opSubtraction(float d1, float d2) { return max(-d1, d2); }
float opSmoothSubtraction(float d1, float d2, float k) {
    float h = clamp(0.5 - 0.5 * (d2 + d1) / k, 0.0, 1.0);
    return lerp(d2, -d1, h) + k * h * (1.0 - h);
}

float opIntersection(float d1, float d2) { return max(d1, d2); }
float opSmoothIntersection(float d1, float d2, float k) {
    float h = clamp(0.5 - 0.5 * (d2 - d1) / k, 0.0, 1.0);
    return lerp(d2, d1, h) + k * h * (1.0 - h);
}

//----------symmetry----------
float2 opSymY(in float2 p)
{
    return float2(abs(p.x), p.y);
}

float2 opSymX(in float2 p)
{
    return float2(p.x, abs(p.y));
}

float2 opSymXS(in float2 p)
{
    return float2(p.x * sign(p.y), abs(p.y));
}

float2 opSym45(in float2 p)
{
    p.xy *= sign(p.y - p.x); return p;
}

#endif