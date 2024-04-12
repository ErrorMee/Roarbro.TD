using UnityEngine;

public static class ShakeUtil
{
    /// <summary>
    /// 播放震动
    /// </summary>
    /// <param name="transform">相机或者其他</param>
    /// <param name="offsetMax">位移幅度</param>
    /// <param name="autoClear">结束时自动销毁</param>
    /// <param name="stress">强度</param>
    /// <param name="recoveryVelocity">恢复速率 1/recoveryVelocity 就是时长</param>
    /// <param name="frequency">振频</param>
    /// <param name="traumaExponent">曲线指数</param>
    public static void Play(Transform transform, Vector3 offsetMax,
        bool autoClear = true, float stress = 1, float recoveryVelocity = 1.5f, float frequency = 25, float traumaExponent = 1)
    {
        ShakeableTransform shakeableTransform = transform.gameObject.GetOrAddComponent<ShakeableTransform>();
        shakeableTransform.offsetMax = offsetMax;

        shakeableTransform.autoClear = autoClear;
        shakeableTransform.recoverySpeed = recoveryVelocity;
        shakeableTransform.frequency = frequency;
        shakeableTransform.traumaExponent = traumaExponent;

        shakeableTransform.InduceStress(stress);
    }

    /// <summary>
    /// 清理震动
    /// </summary>
    /// <param name="transform"></param>
    public static void Clear(Transform transform)
    {
        ShakeableTransform shakeableTransform = transform.GetComponent<ShakeableTransform>();
        if (shakeableTransform)
        {
            shakeableTransform.Clear();
            Object.Destroy(shakeableTransform);
        }
    }
}

[DisallowMultipleComponent]
public class ShakeableTransform : MonoBehaviour
{
    /// <summary>
    /// 振频
    /// </summary>
    public float frequency = 25;
    /// <summary>
    /// 位移幅度
    /// </summary>
    public Vector3 offsetMax = Vector3.one * 0.5f;

    private float seed;
    /// <summary>
    /// 时长
    /// </summary>
    public float recoverySpeed = 1.5f;

    /// <summary>
    /// 强度
    /// </summary>
    private float trauma = 0;

    /// <summary>
    /// 曲线指数
    /// </summary>
    public float traumaExponent = 2;

    private Vector3 origLocalPos;
    /// <summary>
    /// 结束时自动销毁
    /// </summary>
    public bool autoClear = true;

    private void Awake()
    {
        seed = Random.value;
        origLocalPos = transform.localPosition;
    }

    public void InduceStress(float stress)
    {
        trauma = Mathf.Clamp01(trauma + stress);
    }

    private void Update()
    {
        if (trauma == 0)
        {
            if (autoClear)
            {
                Clear();
            }
            return;
        }

        float shake = Mathf.Pow(trauma, traumaExponent);

        float bilinY = Time.time * frequency;

        float bilin0 = Mathf.PerlinNoise(seed, bilinY) * 2 - 1;
        float bilin1 = Mathf.PerlinNoise(seed + 1, bilinY) * 2 - 1;
        float bilin2 = Mathf.PerlinNoise(seed + 2, bilinY) * 2 - 1;

        //if (maximumTranslationShake.magnitude > 0)
        {
            transform.localPosition = new Vector3(
                offsetMax.x * bilin0,
                offsetMax.y * bilin1,
                offsetMax.z * bilin2
            ) * shake + origLocalPos;
        }
        
        trauma = Mathf.Clamp01(trauma - recoverySpeed * Time.deltaTime);
    }

    public void Clear()
    {
        transform.localPosition = origLocalPos;
    }
}