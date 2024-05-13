using UnityEngine;

public partial class EnemyUnit : WorldUnit
{
    float moveSpeed = 0.01f;

    private void MoveEnter()
    {
    }

    private void MoveUpdate()
    {
        transform.localPosition += Vector3.back * moveSpeed;

        if (transform.localPosition.z <= -5)
        {
            fsm.ChangeState(EnemyState.Die);
        }
    }
}