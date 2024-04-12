using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RayUtil
{
    public static Vector3 FixedY(Ray ray, float y, float offsetMagnitude = 0)
    {
        Vector3 origin = ray.origin;
        Vector3 direction = ray.direction;

        float yOffset = origin.y - y;

        float magnitude = Mathf.Abs(yOffset / direction.y);

        return origin + direction * (magnitude + offsetMagnitude);
    }
}
