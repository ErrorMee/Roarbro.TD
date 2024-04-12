using System;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    /// <summary>
    /// �ٶ�
    /// </summary>
    [NonSerialized] public Vector3 velocity;
    /// <summary>
    /// �������
    /// </summary>
    [Range(0.1f, 64)]
    public float maxSpeed;

    /// <summary>
    /// ����
    /// </summary>
    protected Vector3 steerForce;
    /// <summary>
    /// �����
    /// </summary>
    [Range(0.1f, 32)]
    public float maxForce;

    public static Rect stage;
    public EdgeBehavior edgeBehavior = EdgeBehavior.Pass;

    public Vehicle()
    {
    }

    public virtual void Render()
    {
        float deltaTime = Time.deltaTime;
        steerForce = Vector3.ClampMagnitude(steerForce, maxForce);
        velocity += steerForce * deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        transform.position += velocity * deltaTime;
        transform.position = EdgeBehaviorUtil.CorrectPosition(edgeBehavior, stage, transform.position, ref velocity);

        steerForce = Vector3.zero;
    }
}