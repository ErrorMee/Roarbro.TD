using UnityEngine;

public static class AngleUtil
{
    public static float HalfPI = Mathf.PI * 0.5f;
    public static float ThreeHalfPI = Mathf.PI * 1.5f;
    public static float TwoPI = Mathf.PI * 2;

    /// <summary>
    /// 0-360
    /// </summary>
    /// <param name="dira"></param>
    /// <param name="dirb"></param>
    /// <returns></returns>
    public static float AngleXZ(Vector3 dira, Vector3 dirb)
    {
        return Vector2.Angle(dira.XZ(), dirb.XZ());
    }

    /// <summary>
    /// -360 - 360
    /// </summary>
    /// <param name="dira"></param>
    /// <param name="dirb"></param>
    /// <returns></returns>
    public static float SignedAngleXZ(Vector3 dira, Vector3 dirb)
    {
        return Vector2.SignedAngle(dira.XZ(), dirb.XZ());
    }

    public static float SignedAngleXZ(this Vector3 dir)
    {
        return Vector2.SignedAngle(dir.XZ(), Vector2.up);
    }
}
