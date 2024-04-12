//https://iquilezles.org/articles/distfunctions/
#if !defined(MY_MATH)
#define MY_MATH

//Packages/com.unity.render-pipelines.core/ShaderLibrary/Macros.hlsl
//#define FOUR_PI     12.5663706143591729538
//#define TWO_PI      6.28318530717958647693
//#define PI          3.14159265358979323846
//#define HALF_PI     1.57079632679489661923
//#define LOG2_E      1.44269504088896340736
//#define PI_DIV_FOUR 0.78539816339744830961  
//#define INV_SQRT2   0.70710678118654752440
//#define INV_HALF_PI 0.63661977236758134308
//#define INV_PI      0.31830988618379067154
//#define INV_TWO_PI  0.15915494309189533577
//#define INV_FOUR_PI 0.07957747154594766788
#define PI_DIV_TRI 1.0471975512//PI/3
#define PI_DIV_SIX 0.5235987756//PI/6
#define PI_DIV_OCT 0.3926990817//PI/8
#define PI_DIV_DOZ 0.2617993878//PI/12

//step(a, b) if b < a, return 0 else return 1
float ALessB(float a, float b) { return step(a, b); }
float AGreatB(float a, float b) { return step(b, a); }

float dot2(float2 v) { return dot(v, v); }
float dot2(in float3 v) { return dot(v, v); }
float ndot(in float2 a, in float2 b) { return a.x * b.x - a.y * b.y; }

float2 rotate(float2 p, float radian)
{
    float s; float c; sincos(radian, s, c);
    return float2(p.x * c - p.y * s, p.x * s + p.y * c);
}

#endif