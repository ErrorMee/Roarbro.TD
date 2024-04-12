using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseUtil
{
    /// <summary>
    /// 噪声图，代码访问需要readable
    /// </summary>
    public static Texture2D noiseSource;

    public const float mapRadius = 8.0f;
    public const float mapDiameter = mapRadius * 2;
    /// <summary>
    /// 噪声采样
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static Vector4 SampleNoise(Vector3 position)
    {
        float u = (mapRadius + position.x + position.y) / mapDiameter;
        float v = (mapRadius + position.z + position.y) / mapDiameter;
        return noiseSource.GetPixelBilinear(u, v);
    }

    /// <summary>
    /// 扰乱Mesh
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static Vector3 NoiseVector3(Vector3 position,
        float xNoiseStrength, float yNoiseStrength, float zNoiseStrength)
    {
        Vector4 sample = SampleNoise(position);
        position.x += (sample.x * 2 - 1) * xNoiseStrength;
        position.y += (sample.y * 2 - 1) * yNoiseStrength;
        position.z += (sample.z * 2 - 1) * zNoiseStrength;
        return position;
    }
}
