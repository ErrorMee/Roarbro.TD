using UnityEngine;

public partial class EnemyUnit : WorldUnit
{
    float moveSpeed = 0.01f;

    private void MoveEnter()
    {
    }

    private void MoveUpdate()
    {
        Vector3 newPos = transform.localPosition + Vector3.back * moveSpeed;
        transform.LookAt(newPos);
        GridModel.Instance.UpdatePos(this, newPos);

        if (transform.localPosition.z <= GridUtil.FightEndY)
        {
            fsm.ChangeState(EnemyState.Die);
        }
    }
}