//https://www.shadertoy.com/view/4djSRW
//https://www.shadertoy.com/playlist/fXlXzf&from=0&num=12
#if !defined(MY_NOISE_INCLUDED)
#define MY_NOISE_INCLUDED

float hash11(float p)
{
    p = frac(p * 0.1031);
    p *= p + 33.33;
    p *= p + p;
    return frac(p);
}

float hash12(float2 p)
{
    float3 p3 = frac(float3(p.xyx) * 0.1031);
    p3 += dot(p3, p3.yzx + 33.33);
    return frac((p3.x + p3.y) * p3.z);
}

float2 hash2(float2 p)
{
    // procedural white noise	
    return frac(sin(float2(dot(p,float2(127.1,311.7)),dot(p,float2(269.5,183.3))))*43758.5453);
}

float3 hash3(float2 p)
{
    float3 q = float3(dot(p, float2(127.1, 311.7)),
        dot(p, float2(269.5, 183.3)),
        dot(p, float2(419.2, 371.9)));
    return frac(sin(q) * 43758.5453);
}

float voronoise(in float2 p, float u = 1, float v = 0)
{
    float k = 1.0 + 63.0 * pow(1.0 - v, 6.0);

    float2 i = floor(p);
    float2 f = frac(p);

    float2 a = float2(0.0, 0.0);
    for (int y = -2; y <= 2; y++)
        for (int x = -2; x <= 2; x++)
        {
            float2  g = float2(x, y);
            float3  o = hash3(i + g) * float3(u, u, 1.0);
            float2  d = g - f + o.xy;
            float w = pow(abs(1.0 - smoothstep(0.0, 1.414, length(d))), k);
            a += float2(o.z * w, w);
        }

    return a.x / a.y;
}

float3 voronoi(in float2 x)
{
    float2 n = floor(x);
    float2 f = frac(x);

    //----------------------------------
    // first pass: regular voronoi
    //----------------------------------
    float2 mg, mr;

    float md = 8.0;
    int j; int i;
    for (j = -1; j <= 1; j++)
        for (i = -1; i <= 1; i++)
        {
            float2 g = float2(float(i), float(j));
            float2 o = hash2(n + g);
            float2 r = g + o - f;
            float d = dot(r, r);

            if (d < md)
            {
                md = d;
                mr = r;
                mg = g;
            }
        }

    //----------------------------------
    // second pass: distance to borders
    //----------------------------------
    md = 8.0;
    for (j = -2; j <= 2; j++)
        for (i = -2; i <= 2; i++)
        {
            float2 g = mg + float2(float(i), float(j));
            float2 o = hash2(n + g);
            float2 r = g + o - f;

            if (dot(mr - r, mr - r) > 0.00001)
                md = min(md, dot(0.5 * (mr + r), normalize(r - mr)));
        }

    return float3(md, mr);
}

float2 hashSimplex(float2 p) // replace this by something better
{
    p = float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)));
    return -1.0 + 2.0 * frac(sin(p) * 43758.5453123);
}

float SimplexNoise(in float2 p)
{
    const float K1 = 0.366025404; // (sqrt(3)-1)/2;
    const float K2 = 0.211324865; // (3-sqrt(3))/6;

    float2  i = floor(p + (p.x + p.y) * K1);
    float2  a = p - i + (i.x + i.y) * K2;
    float m = step(a.y, a.x);
    float2  o = float2(m, 1.0 - m);
    float2  b = a - o + K2;
    float2  c = a - 1.0 + 2.0 * K2;
    float3  h = max(0.5 - float3(dot(a, a), dot(b, b), dot(c, c)), 0.0);
    float3  n = h * h * h * h * float3(dot(a, hashSimplex(i + 0.0)), dot(b, hashSimplex(i + o)), dot(c, hashSimplex(i + 1.0)));
    return dot(n, float3(70.0, 70.0, 70.0));
}


float hashValue(float2 p)  // replace this by something better
{
    p = 50.0 * frac(p * 0.3183099 + float2(0.71, 0.113));
    return -1.0 + 2.0 * frac(p.x * p.y * (p.x + p.y));
}

float noiseValue(in float2 p)
{
    float2 i = floor(p);
    float2 f = frac(p);

    float2 u = f * f * (3.0 - 2.0 * f);

    return lerp(lerp(hashValue(i + float2(0.0, 0.0)),
        hashValue(i + float2(1.0, 0.0)), u.x),
        lerp(hashValue(i + float2(0.0, 1.0)),
            hashValue(i + float2(1.0, 1.0)), u.x), u.y);
}

float2 hashGradient( float2 z)  // replace this anything that returns a random vector
{
    // 2D to 1D  (feel free to replace by some other)
    int n = z.x + z.y * 11111;

    // Hugo Elias hash (feel free to replace by another one)
    n = (n << 13) ^ n;
    n = (n * (n * n * 15731 + 789221) + 1376312589) >> 16;
    float s; float c; sincos(n, s, c);
    return float2(c, s);                             
}

float noiseGradient(in float2 p)
{
    float2 i = float2(floor(p));
    float2 f = frac(p);

    float2 u = f * f * (3.0 - 2.0 * f); // feel free to replace by a quintic smoothstep instead

    return lerp(lerp(dot(hashGradient(i + float2(0, 0)), f - float2(0.0, 0.0)),
        dot(hashGradient(i + float2(1, 0)), f - float2(1.0, 0.0)), u.x),
        lerp(dot(hashGradient(i + float2(0, 1)), f - float2(0.0, 1.0)),
            dot(hashGradient(i + float2(1, 1)), f - float2(1.0, 1.0)), u.x), u.y);
}


float2 hashWave(float2 n) { return sin(n.x * n.y * float2(12, 17) + float2(1, 2)); }

float noiseWave(float2 p)
{
    const float kF = 2.0;  // make 6 to see worms 2

    float2 i = floor(p);
    float2 f = frac(p);
    f = f * f * (3.0 - 2.0 * f);
    return lerp(lerp(sin(kF * dot(p, hashWave(i + float2(0, 0)))),
        sin(kF * dot(p, hashWave(i + float2(1, 0)))), f.x),
        lerp(sin(kF * dot(p, hashWave(i + float2(0, 1)))),
            sin(kF * dot(p, hashWave(i + float2(1, 1)))), f.x), f.y);
}

float squares16(uint ctr) {
    const uint key = uint(0x7a1a912f);
    const float two16 = 65536.0;

    uint x, y, z;
    y = ctr * key;
    z = (ctr + uint(1)) * key;

    x = y;

    x = x * x + y; x = (x >> 16) | (x << 16);
    x = x * x + z; x = (x >> 16) | (x << 16);
    x = (x * x + y) >> 16;
    return float(x) / two16;
}

#endif