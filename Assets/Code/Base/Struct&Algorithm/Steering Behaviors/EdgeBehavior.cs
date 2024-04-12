using System;
using UnityEngine;

/// <summary>
/// ��Ե��Ϊ
/// </summary>
public enum EdgeBehavior
{
    /// <summary>
    /// ��������
    /// </summary>
    Pass,
    /// <summary>
    /// ��ס
    /// </summary>
    Stop,
    /// <summary>
    /// ���������ܻ���һ�������˶�
    /// </summary>
    Shore,
    /// <summary>
    /// �ƻ�
    /// </summary>
    Wrap,
    /// <summary>
    /// ����
    /// </summary>
    Bounce,
    
}

public static class EdgeBehaviorUtil
{
    public static Vector3 CorrectPosition(EdgeBehavior edgeBehavior, Rect stage, 
        Vector3 position, ref Vector3 velocity)
    {
        switch (edgeBehavior)
        {
            case EdgeBehavior.Stop:
                float deltaTime = Time.deltaTime;
                if (position.x > stage.xMax)
                {
                    position.x = stage.xMax;
                    position.z -= velocity.z * deltaTime;
                }
                if (position.x < stage.xMin)
                {
                    position.x = stage.xMin;
                    position.z -= velocity.z * deltaTime;
                }
                if (position.z > stage.yMax)
                {
                    position.z = stage.yMax;
                    position.x -= velocity.x * deltaTime;
                }
                if (position.z < stage.yMin)
                {
                    position.z = stage.yMin;
                    position.x -= velocity.x * deltaTime;
                } 
                break;
            case EdgeBehavior.Shore:
                if (position.x > stage.xMax) position.x = stage.xMax;
                if (position.x < stage.xMin) position.x = stage.xMin;
                if (position.z > stage.yMax) position.z = stage.yMax;
                if (position.z < stage.yMin) position.z = stage.yMin;
                break;
            case EdgeBehavior.Wrap:
                if (position.x > stage.xMax) position.x = stage.xMin;
                if (position.x < stage.xMin) position.x = stage.xMax;
                if (position.z > stage.yMax) position.z = stage.yMin;
                if (position.z <  stage.yMin) position.z = stage.yMax;
                break;
            case EdgeBehavior.Bounce:
                if (position.x > stage.xMax)
                {
                    position.x = stage.xMax;
                    velocity.x *= -1;
                }
                else if (position.x < stage.xMin)
                {
                    position.x = stage.xMin;
                    velocity.x *= -1;
                }
                if (position.z > stage.yMax)
                {
                    position.z = stage.yMax;
                    velocity.z *= -1;
                }
                else if (position.z < stage.yMin)
                {
                    position.z = stage.yMin;
                    velocity.z *= -1;
                }
                break;
        }
        return position;
    }
}