using System.Collections.Generic;
using UnityEngine;
//https://www.zhihu.com/question/53708752
public static class Spline
{
	public static Vector3 Bezier(Vector3 start, Vector3 control, Vector3 end, float t)
	{
		float r = 1f - t;
		return r * r * start + 2f * r * t * control + t * t * end;
	}

    public static Vector2 Bezier(Vector2 start, Vector2 control, Vector2 end, float t)
    {
        float r = 1f - t;
        return r * r * start + 2f * r * t * control + t * t * end;
    }

    public static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        Vector3 v0 = (p2 - p0) * 0.5f;
        Vector3 v1 = (p3 - p1) * 0.5f;

        float a0 = 2 * t3 - 3 * t2 + 1;
        float a1 = t3 - 2 * t2 + t;
        float a2 = t3 - t2;
        float a3 = -2 * t3 + 3 * t2;

        return a0 * p1 + a1 * v0 + a2 * v1 + a3 * p2;
    }

    public static Vector2 CatmullRom(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        Vector2 v0 = (p2 - p0) * 0.5f;
        Vector2 v1 = (p3 - p1) * 0.5f;

        float a0 = 2 * t3 - 3 * t2 + 1;
        float a1 = t3 - 2 * t2 + t;
        float a2 = t3 - t2;
        float a3 = -2 * t3 + 3 * t2;

        return a0 * p1 + a1 * v0 + a2 * v1 + a3 * p2;
    }

    public static float CatmullRom(float p0, float p1, float p2, float p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        float v0 = (p2 - p0) * 0.5f;
        float v1 = (p3 - p1) * 0.5f;

        float a0 = 2 * t3 - 3 * t2 + 1;
        float a1 = t3 - 2 * t2 + t;
        float a2 = t3 - t2;
        float a3 = -2 * t3 + 3 * t2;

        return a0 * p1 + a1 * v0 + a2 * v1 + a3 * p2;
    }
}
