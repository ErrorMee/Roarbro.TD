using System;
using UnityEngine;

public static class Vector3To2Util
{
    public static Vector2 XZ(this Vector3 self)
    {
        return new Vector2(self.x, self.z);
    }

    public static Vector2 XY(this Vector3 self)
    {
        return new Vector2(self.x, self.y);
    }

    public static Vector3 AddXZ(this Vector3 self, Vector3 vector3)
    {
        return new Vector3(self.x + vector3.x, self.y, self.z + vector3.z);
    }

    public static Vector3 SubXZ(this Vector3 self, Vector3 vector3)
    {
        return new Vector3(self.x - vector3.x, self.y, self.z - vector3.z);
    }

    public static Vector3 MutXZ(this Vector3 self, float d)
    {
        return new Vector3(self.x * d, self.y, self.z * d);
    }

    public static Vector3 DvdXZ(this Vector3 self, float d)
    {
        return new Vector3(self.x / d, self.y, self.z / d);
    }

    public static Vector3 NormalizeXZ(this Vector3 self)
    {
        float num = self.MagnitudeXZ();
        if (num > 1E-05f)
        {
            return self.DvdXZ(num);
        }
        return Vector3.zero;
    }

    public static float DotXZ(this Vector3 self, Vector3 vector3)
    {
        return self.x * vector3.x + self.z * vector3.z;
    }

    public static float MagnitudeXY(this Vector3 self)
    {
        return (float)Math.Sqrt(self.x * self.x + self.y * self.y);
    }

    public static float MagnitudeXZ(this Vector3 self)
    {
        return (float)Math.Sqrt(self.x * self.x + self.z * self.z);
    }

    public static float MagnitudeXZSqr(this Vector3 self)
    {
        return self.x * self.x + self.z * self.z;
    }
}