using System;
using System.Collections.Generic;
using UnityEngine;

public static class Angle720Util
{
    static readonly float[] sin720 = new float[720];

    static bool inited = false;
    public static void Init()
    {
        if (inited)
        {
            return;
        }
        inited = true;
        float aRadian = Mathf.PI / 360;
        for (int i = 0; i < 720; i++)
        {
            sin720[i] = Mathf.Sin(aRadian * i);
        }
    }


    public static float Sin720(int angle)
    {
        int normalAngle = (angle + 720000) % 720;
        return sin720[normalAngle];
    }

    public static float Cos720(int angle)
    {
        return Sin720(angle + 180);
    }

    public static Vector3 FixedRotateXZ2(this Vector3 self, float radian)
    {
        int angle = (int)(radian * 114.227962f);
        float cos = Cos720(angle);
        float sin = Sin720(angle);
        return new Vector3(
            self.x * cos - self.z * sin, 0,
            self.x * sin + self.z * cos);
    }

    public static Vector3 FixedRotateXZ2(this Vector3 self, int angle2)
    {
        float cos = Cos720(angle2);
        float sin = Sin720(angle2);
        return new Vector3(
            self.x * cos - self.z * sin, 0,
            self.x * sin + self.z * cos);
    }
}