//https://iquilezles.org/articles/distfunctions2d/
//https://www.shadertoy.com/playlist/MXdSRf
#if !defined(MY_SDF2D_INCLUDED)
#define MY_SDF2D_INCLUDED

#include "SDF.hlsl"

float sdCircle(float2 p, float r)
{
    return length(p) - r;
}

float sdBox(in float2 p, in float2 b)
{
    float2 d = abs(p) - b;
    return length(max(d, 0.0)) + min(max(d.x, d.y), 0.0);
}

float sdRoundedBox(in float2 p, in float2 box, in float4 roundness)
{
    roundness.xy = (p.x > 0.0) ? roundness.xy : roundness.zw;
    roundness.x = (p.y > 0.0) ? roundness.x : roundness.y;
    float2 q = abs(p) - box + roundness.x;
    return min(max(q.x, q.y), 0.0) + length(max(q, 0.0)) - roundness.x;
}

float sdOrientedBox(in float2 p, in float2 a, in float2 b, float th)
{
    float l = length(b - a);
    float2  d = float2(b.x - a.x, a.y - b.y) / l;
    float2  q = (p - (a + b) * 0.5);
    q = mul(float2x2(d.x, -d.y, d.y, d.x), q);
    q = abs(q) - float2(l, th) * 0.5;
    return length(max(q, 0.0)) + min(max(q.x, q.y), 0.0);
}

float sdSegment(in float2 p, in float2 a, in float2 b)
{
    float2 pa = p - a, ba = b - a;
    float h = clamp(dot(pa, ba) / dot(ba, ba), 0.0, 1.0);
    return length(pa - ba * h);
}

float sdRhombus(in float2 p, in float2 b)
{
    p = abs(p);
    float h = clamp(ndot(b - 2.0 * p, b) / dot(b, b), -1.0, 1.0);
    float d = length(p - 0.5 * b * float2(1.0 - h, 1.0 + h));
    return d * sign(p.x * b.y + p.y * b.x - b.x * b.y);
}

float sdTrapezoid(in float2 p, in float r1, float r2, float he)
{
    float2 k1 = float2(r2, he);
    float2 k2 = float2(r2 - r1, 2.0 * he);
    p.x = abs(p.x);
    float2 ca = float2(p.x - min(p.x, (p.y < 0.0) ? r1 : r2), abs(p.y) - he);
    float2 cb = p - k1 + k2 * clamp(dot(k1 - p, k2) / dot2(k2), 0.0, 1.0);
    float s = (cb.x < 0.0 && ca.y < 0.0) ? -1.0 : 1.0;
    return s * sqrt(min(dot2(ca), dot2(cb)));
}

//(width, height, skew)
float sdParallelogram(in float2 p, float wi, float he, float sk)
{
    float2 e = float2(sk, he);
    p = (p.y < 0.0) ? -p : p;
    float2  w = p - e; w.x -= clamp(w.x, -wi, wi);
    float2  d = float2(dot(w, w), -w.y);
    float s = p.x * e.y - p.y * e.x;
    p = (s < 0.0) ? -p : p;
    float2  v = p - float2(wi, 0); v -= e * clamp(dot(v, e) / dot(e, e), -1.0, 1.0);
    d = min(d, float2(dot(v, v), wi * he - abs(s)));
    return sqrt(d.x) * sign(-d.y);
}

float sdTriangle(in float2 p, in float2 p0, in float2 p1, in float2 p2)
{
    float2 e0 = p1 - p0, e1 = p2 - p1, e2 = p0 - p2;
    float2 v0 = p - p0, v1 = p - p1, v2 = p - p2;
    float2 pq0 = v0 - e0 * clamp(dot(v0, e0) / dot(e0, e0), 0.0, 1.0);
    float2 pq1 = v1 - e1 * clamp(dot(v1, e1) / dot(e1, e1), 0.0, 1.0);
    float2 pq2 = v2 - e2 * clamp(dot(v2, e2) / dot(e2, e2), 0.0, 1.0);
    float s = sign(e0.x * e2.y - e0.y * e2.x);
    float2 d = min(min(float2(dot(pq0, pq0), s * (v0.x * e0.y - v0.y * e0.x)),
        float2(dot(pq1, pq1), s * (v1.x * e1.y - v1.y * e1.x))),
        float2(dot(pq2, pq2), s * (v2.x * e2.y - v2.y * e2.x)));
    return -sqrt(d.x) * sign(d.y);
}

float sdEquilateralTriangle(in float2 p, in float r)
{
    const float k = sqrt(3.0);
    p.x = abs(p.x) - r;
    p.y = p.y + r / k;
    if (p.x + k * p.y > 0.0) p = float2(p.x - k * p.y, -k * p.x - p.y) / 2.0;
    p.x -= clamp(p.x, -2.0 * r, 0.0);
    return -length(p) * sign(p.y);
}

float sdTriangleIsosceles(in float2 p, in float2 q)
{
    p.x = abs(p.x);
    float2 a = p - q * clamp(dot(p, q) / dot(q, q), 0.0, 1.0);
    float2 b = p - q * float2(clamp(p.x / q.x, 0.0, 1.0), 1.0);
    float s = -sign(q.y);
    float2 d = min(float2(dot(a, a), s * (p.x * q.y - p.y * q.x)),
        float2(dot(b, b), s * (p.y - q.y)));
    return -sqrt(d.x) * sign(d.y);
}

float sdUnevenCapsule(float2 p, float r1, float r2, float h)
{
    p.x = abs(p.x);
    float b = (r1 - r2) / h;
    float a = sqrt(1.0 - b * b);
    float k = dot(p, float2(-b, a));
    if (k < 0.0) return length(p) - r1;
    if (k > a * h) return length(p - float2(0.0, h)) - r2;
    return dot(p, float2(a, b)) - r1;
}

float sdPentagon(in float2 p, in float r)
{
    const float3 k = float3(0.809016994, 0.587785252, 0.726542528);
    p.x = abs(p.x);
    p -= 2.0 * min(dot(float2(-k.x, k.y), p), 0.0) * float2(-k.x, k.y);
    p -= 2.0 * min(dot(float2(k.x, k.y), p), 0.0) * float2(k.x, k.y);
    p -= float2(clamp(p.x, -r * k.z, r * k.z), r);
    return length(p) * sign(p.y);
}

float sdHexagon(in float2 p, in float r)
{
    const float3 k = float3(-0.866025404, 0.5, 0.577350269);
    p = abs(p);
    p -= 2.0 * min(dot(k.xy, p), 0.0) * k.xy;
    p -= float2(clamp(p.x, -k.z * r, k.z * r), r);
    return length(p) * sign(p.y);
}

float sdQuadraticCircle(in float2 p, float r = 1)
{
    p = abs(p / r); if (p.y > p.x) p = p.yx;

    float a = p.x - p.y;
    float b = p.x + p.y;
    float c = (2.0 * b - 1.0) / 3.0;
    float h = sqrt(max(a * a + c * c * c, 0.0));
    float u = pow(max(h - a, 0.0), 1.0 / 3.0);
    float v = pow(abs(h + a), 1.0 / 3.0);
    float t = (u - v) * 0.5;
    float2 w = float2(-t, t) + 0.75 - t * t - p;
    return length(w * r) * sign(a * a * 0.5 + b - 1.5);
}

float sdQuad(in float2 p, in float2 p0, in float2 p1, in float2 p2, in float2 p3)
{
    float2 e0 = p1 - p0; float2 v0 = p - p0;
    float2 e1 = p2 - p1; float2 v1 = p - p1;
    float2 e2 = p3 - p2; float2 v2 = p - p2;
    float2 e3 = p0 - p3; float2 v3 = p - p3;

    float2 pq0 = v0 - e0 * clamp(dot(v0, e0) / dot(e0, e0), 0.0, 1.0);
    float2 pq1 = v1 - e1 * clamp(dot(v1, e1) / dot(e1, e1), 0.0, 1.0);
    float2 pq2 = v2 - e2 * clamp(dot(v2, e2) / dot(e2, e2), 0.0, 1.0);
    float2 pq3 = v3 - e3 * clamp(dot(v3, e3) / dot(e3, e3), 0.0, 1.0);

    float2 ds = min(min(float2(dot(pq0, pq0), v0.x * e0.y - v0.y * e0.x),
        float2(dot(pq1, pq1), v1.x * e1.y - v1.y * e1.x)),
        min(float2(dot(pq2, pq2), v2.x * e2.y - v2.y * e2.x),
            float2(dot(pq3, pq3), v3.x * e3.y - v3.y * e3.x)));

    float d = sqrt(ds.x);

    return (ds.y > 0.0) ? -d : d;
}

float sdSquircle(float2 p, float n)
{
    // symmetries
    p = abs(p); if (p.y > p.x) p = p.yx;

    n = 2.0 / n; // note the remapping in order to match the

    const int num = 16; // tesselate into 8x16=128 segments, more denselly at the corners
    float s = 1.0;
    float d = 1e20;
    float2 oq = float2(1.0, 0.0);
    for (int i = 1; i <= num; i++)
    {
        float h = float(i) / float(num);
        float an = (6.283185 / 8.0) * h;
        float s; float c;
        sincos(an, s, c);
        float2  q = pow(float2(c, s), float2(n, n));
        float2  pa = p - oq;
        float2  ba = q - oq;
        float2  z = pa - ba * clamp(dot(pa, ba) / dot(ba, ba), 0.0, 1.0);
        float d2 = dot(z, z);
        if (d2 < d)
        {
            d = d2;
            s = pa.x * ba.y - pa.y * ba.x;
        }
        oq = q;
    }
    return sqrt(d) * sign(s);
}

float sdStar5(in float2 p, in float r, in float rf)
{
    const float2 k1 = float2(0.809016994375, -0.587785252292);
    const float2 k2 = float2(-k1.x, k1.y);
    p.x = abs(p.x);
    p -= 2.0 * max(dot(k1, p), 0.0) * k1;
    p -= 2.0 * max(dot(k2, p), 0.0) * k2;
    p.x = abs(p.x);
    p.y -= r;
    float2 ba = rf * float2(-k1.y, k1.x) - float2(0, 1);
    float h = clamp(dot(p, ba) / dot(ba, ba), 0.0, r);
    return length(p - ba * h) * sign(p.y * ba.x - p.x * ba.y);
}

float sdStar(in float2 p, in float r, in int n, in float m) // m=[2,n]
{
    // these 4 lines can be precomputed for a given shape
    float an = 3.141593 / float(n);
    float en = 3.141593 / m;
    float2  acs = float2(cos(an), sin(an));
    float2  ecs = float2(cos(en), sin(en)); // ecs=float2(0,1) and simplify, for regular polygon,
    //float2 ecs = float2(0, 1);// simplify, for regular polygon,

    // reduce to first sector
    float bn = fmod(atan2(abs(p.x), p.y), 2.0 * an) - an;
    p = length(p) * float2(cos(bn), abs(sin(bn)));

    // line sdf
    p -= r * acs;
    p += ecs * clamp(-dot(p, ecs), 0.0, r * acs.y / ecs.y);
    return length(p) * sign(p.x);
}

//w=width; l=len; a=angle
float sdJoint2DSphere(in float2 p, in float l, in float a, float w)
{
    // parameters
    float2  sc = float2(sin(a), cos(a));
    float ra = 0.5 * l / a;

    // recenter
    p.x -= ra;

    // reflect
    float2 q = p - 2.0 * sc * max(0.0, dot(sc, p));

    // distance
    float u = abs(ra) - length(q);
    float d = (q.y < 0.0) ? length(q + float2(ra, 0.0)) : abs(u);

    return d - w;
}

//b is control
float sdBezier(in float2 pos, in float2 A, in float2 B, in float2 C)
{
    float2 a = B - A;
    float2 b = A - 2.0 * B + C;
    float2 c = a * 2.0;
    float2 d = A - pos;
    float kk = 1.0 / dot(b, b);
    float kx = kk * dot(a, b);
    float ky = kk * (2.0 * dot(a, a) + dot(d, b)) / 3.0;
    float kz = kk * dot(d, a);
    float res = 0.0;
    float p = ky - kx * kx;
    float p3 = p * p * p;
    float q = kx * (2.0 * kx * kx - 3.0 * ky) + kz;
    float h = q * q + 4.0 * p3;
    if (h >= 0.0)
    {
        h = sqrt(h);
        float2 x = (float2(h, -h) - q) / 2.0;
        float ti = 1.0 / 3.0;
        float2 uv = sign(x) * pow(abs(x), float2(ti, ti));
        float t = clamp(uv.x + uv.y - kx, 0.0, 1.0);
        float2 pdd = d + (c + b * t) * t;
        res = dot(pdd, pdd);
    }
    else
    {
        float z = sqrt(-p);
        float v = acos(q / (p * z * 2.0)) / 3.0;
        float m = cos(v);
        float n = sin(v) * 1.732050808;
        float3  t = clamp(float3(m + m, -n - m, n - m) * z - kx, 0.0, 1.0);
        float2 pddx = d + (c + b * t.x) * t.x;
        float2 pddy = d + (c + b * t.y) * t.y;
        res = min(dot(pddx, pddx), dot(pddy, pddy));
    }
    return sqrt(res);
}

//b.x = out radius 
//b.y = in radius
//r = round
float sdCross(in float2 p, in float2 b, float r)
{
    p = abs(p); p = (p.y > p.x) ? p.yx : p.xy;
    float2  q = p - b;
    float k = max(q.y, q.x);
    float2  w = (k > 0.0) ? q : float2(b.y - p.x, -k);
    return sign(k) * length(max(w, 0.0)) + r;
}

// k in (0,1) range
float sdHyperbolicCross(in float2 p, float k)
{
    // scale
    float s = 1.0 / k - k;
    p = p * s;
    // symmetry
    p = abs(p);
    p = (p.x > p.y) ? p.yx : p.xy;
    // offset
    p += k;

    // solve quartic (for details see https://www.shadertoy.com/view/ftcyW8)
    float x2 = p.x * p.x / 16.0;
    float y2 = p.y * p.y / 16.0;
    float r = (p.x * p.y - 4.0) / 12.0;
    float q = y2 - x2;
    float h = q * q - r * r * r;
    float u;
    if (h < 0.0)
    {
        float m = sqrt(r);
        u = m * cos(acos(q / (r * m)) / 3.0);
    }
    else
    {
        float m = pow(sqrt(h) + q, 1.0 / 3.0);
        u = (m + r / m) / 2.0;
    }
    float w = sqrt(u + x2);
    float x = p.x / 4.0 - w + sqrt(2.0 * x2 - u + (p.y - x2 * p.x * 2.0) / w / 4.0);

    // clamp arm
    x = max(x, k);

    // compute distance to closest point
    float d = length(p - float2(x, 1.0 / x)) / s;

    // sign
    return p.x * p.y < 1.0 ? -d : d;
}

float sdRoundedX(in float2 p, in float w, in float r)
{
    p = abs(p);
    return length(p - min(p.x + p.y, w) * 0.5) - r;
}

float sdHeart(in float2 p)
{
    p.x = abs(p.x);

    if (p.y + p.x > 1.0)
        return sqrt(dot2(p - float2(0.25, 0.75))) - sqrt(2.0) / 4.0;
    return sqrt(min(dot2(p - float2(0.00, 1.00)),
        dot2(p - 0.5 * max(p.x + p.y, 0.0)))) * sign(p.x - p.y);
}

float sdEgg(in float2 p, in float ra, in float rb)
{
    const float k = sqrt(3.0);
    p.x = abs(p.x);
    float r = ra - rb;
    return ((p.y < 0.0) ? length(float2(p.x, p.y)) - r :
        (k * (p.x + r) < p.y) ? length(float2(p.x, p.y - k * r)) :
        length(float2(p.x + r, p.y)) - 2.0 * r) - rb;
}

float sdMossEgg(in float2 p)//https://www.shadertoy.com/view/wsBBR3
{
    p.x = abs(p.x);
    return ((p.y <= 0.) ? length(p) - 1.0 :
        ((p.y - 1.0) > p.x) ? length(p - float2(0.0, 1.0)) - (2. - sqrt(2.)) :
        length(p - float2(-1.0, 0.0)) - 2.);
}

float sdTunnel(in float2 p, in float2 wh)
{
    p.x = abs(p.x); p.y = -p.y;
    float2 q = p - wh;

    float d1 = dot2(float2(max(q.x, 0.0), q.y));
    q.x = (p.y > 0.0) ? q.x : length(p) - wh.x;
    float d2 = dot2(float2(q.x, max(q.y, 0.0)));
    float d = sqrt(min(d1, d2));

    return (max(q.x, q.y) < 0.0) ? -d : d;
}

float sdHexagram(in float2 p, in float r)
{
    const float4 k = float4(-0.5, 0.8660254038, 0.5773502692, 1.7320508076);
    p = abs(p);
    p -= 2.0 * min(dot(k.xy, p), 0.0) * k.xy;
    p -= 2.0 * min(dot(k.yx, p), 0.0) * k.yx;
    p -= float2(clamp(p.x, r * k.z, r * k.w), r);
    return length(p) * sign(p.y);
}


float sdEllipse(float2 p, float2 ab)
{
    // symmetry
    p = abs(p);

    // find root with Newton solver
    float2 q = ab * (p - ab);
    float w = (q.x < q.y) ? 1.570796327 : 0.0;
    for (int i = 0; i < 5; i++)
    {
        float2 cs = float2(cos(w), sin(w));
        float2 u = ab * float2(cs.x, cs.y);
        float2 v = ab * float2(-cs.y, cs.x);
        w = w + dot(p - u, v) / (dot(p - u, u) + dot(v, v));
    }

    // compute final point and distance
    float d = length(p - ab * float2(cos(w), sin(w)));

    // return signed distance
    return (dot(p / ab, p / ab) > 1.0) ? d : -d;
}
//https://www.shadertoy.com/view/Ws3BWH r circleR l pullCircleR h pullDis
float sdOval(in float2 p, float r, in float l, in float h)
{
    p = abs(p);
    return ((p.y - h) * l > p.x * h) ? length(p - float2(0., h)) - ((r + l) - length(float2(h, l))) :
        length(p + float2(l, 0.)) - (r + l);
}

float sdPie( in float2 p, in float2 c, in float r )
{
    p.x = abs(p.x);
    float l = length(p) - r;
    float m = length(p-c*clamp(dot(p,c),0.0,r)); // c=sin/cos of aperture
    return max(l,m*sign(c.y*p.x-c.x*p.y));
}

float sdVesica(float2 p, float r, float d)
{
    p = abs(p);
    float b = sqrt(r * r - d * d);
    return ((p.y - b) * d > p.x * b) ? length(p - float2(0.0, b))
        : length(p - float2(-d, 0.0)) - r;
}

float sdSpiral(in float2 p, float w, in float k)
{
// w is the width / distance from center to tip
// k is the number of rotations
    // body
    const float kTau = 6.283185307;
    float r = length(p);
    float a = atan2(p.y, p.x);
    float n = floor(0.5 / w + (log2(r / w) * k - a) / kTau);
    float ra = w * exp2((a + kTau * (min(n + 0.0, 0.0) - 0.5)) / k);
    float rb = w * exp2((a + kTau * (min(n + 1.0, 0.0) - 0.5)) / k);
    float d = min(abs(r - ra), abs(r - rb));

    // tip
    return min(d, length(p + float2(w, 0.0)));
}

float sdArc(in float2 p, in float2 sc, in float ra, float rb)
{
    // sc is the sin/cos of the arc's aperture
    p.x = abs(p.x);
    return ((sc.y * p.x > sc.x * p.y) ? length(p - sc * ra) :
        abs(length(p) - ra)) - rb;
}

float sdRing(in float2 p, in float2 n, in float r, float th)
{
    p.x = abs(p.x);

    p = mul(float2x2(n.x, n.y, -n.y, n.x), p);
    return max(abs(length(p) - r) - th * 0.5,
        length(float2(p.x, max(0.0, abs(r - p.y) - th * 0.5))) * sign(p.x));
}

float sdHorseshoe(in float2 p, in float2 c, in float r, in float2 w)
{
    p.x = abs(p.x);
    float l = length(p);
    p = mul(float2x2(-c.x, c.y, c.y, c.x), p);
    p = float2((p.y > 0.0 || p.x > 0.0) ? p.x : l * sign(-c.x),
        (p.x > 0.0) ? p.y : l);
    p = float2(p.x, abs(p.y - r)) - w;
    return length(max(p, 0.0)) + min(0.0, max(p.x, p.y));
}

#endif