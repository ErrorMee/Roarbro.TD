using UnityEngine;

[DisallowMultipleComponent]
public class SteeredVehicle : Vehicle
{
    /// <summary>
    /// Ѱ��
    /// </summary>
    /// <param name="target"></param>
    public void Seek(Vector3 target)
    {
        Vector3 desiredVelocity = target - transform.position;
        desiredVelocity = desiredVelocity.normalized * maxSpeed;
        Vector3 velocityGap = desiredVelocity - velocity;
        steerForce += velocityGap;
    }

    [Range(0.1f, 5)]
    public float arrivalThreshold = 0.5f;
    /// <summary>
    /// ����һ��Ŀ��
    /// </summary>
    /// <param name="target"></param>
    protected void Arrive(Vector3 target)
    {
        Vector3 desiredVelocity = target - transform.position;
        float distance = desiredVelocity.magnitude;

        if (distance > arrivalThreshold)
        {
            desiredVelocity = desiredVelocity.normalized * maxSpeed;
        }
        else
        {
            desiredVelocity = desiredVelocity.normalized * (maxSpeed * distance / arrivalThreshold);
        }

        Vector3 velocityGap = desiredVelocity - velocity;
        steerForce += velocityGap;
    }

    /// <summary>
    /// ׷��
    /// </summary>
    /// <param name="target"></param>
    public void Pursue(Vehicle target) 
    { 
        float lookAheadTime = (transform.position - target.transform.position).magnitude / maxSpeed;
        Vector3 predictedTarget = target.transform.position + (target.velocity * lookAheadTime);
        Seek(predictedTarget);
    }

    /// <summary>
    /// �ܿ�
    /// </summary>
    /// <param name="target"></param>
    public void Flee(Vector3 target)
    {
        Vector3 desiredVelocity = target - transform.position;
        desiredVelocity = desiredVelocity.normalized * maxSpeed;
        Vector3 velocityGap = desiredVelocity - velocity;
        steerForce -= velocityGap;
    }

    /// <summary>
    /// �ӱ�
    /// </summary>
    /// <param name="target"></param>
    public void Escape(Vehicle target)
    {
        float lookAheadTime = (transform.position - target.transform.position).magnitude / maxSpeed;
        Vector3 predictedTarget = target.transform.position + (target.velocity * lookAheadTime);
        Flee(predictedTarget);
    }
}