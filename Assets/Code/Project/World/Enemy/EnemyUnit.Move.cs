using UnityEngine;

public partial class EnemyUnit : WorldUnit
{
    float speedFixedDelta;

    private void MoveEnter()
    {
        speedFixedDelta = info.config.speed * Time.fixedDeltaTime;
    }

    private void MoveUpdate()
    {
        Vector3 newPos = transform.localPosition + speedFixedDelta * Vector3.back;
        //transform.LookAt(newPos);
        GridModel.Instance.UpdatePos(this, newPos);

        if (transform.localPosition.z <= GridUtil.FightEndY)
        {
            fsm.ChangeState(EnemyState.Die);
        }
    }
}