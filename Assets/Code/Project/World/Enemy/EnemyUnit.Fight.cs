using UnityEngine;

public partial class EnemyUnit : WorldUnit
{
    float speedFixedDelta;

    private void FightEnter()
    {
        speedFixedDelta = info.config.speed * Time.fixedDeltaTime;
    }

    private void FightUpdate()
    {
        Vector3 newPos = transform.localPosition + speedFixedDelta * Vector3.back;
        //transform.LookAt(newPos);
        GridModel.Instance.UpdatePos(this, newPos);

        if (transform.localPosition.z <= GridUtil.FightEndY)
        {
            fsm.ChangeState(EnemyState.Die);
        }
    }

    public void Attacked(BallInfo ballInfo)
    {
        info.leftHP -= ballInfo.GetAttackValue();
        if (info.leftHP <= 0)
        {
            fsm.ChangeState(EnemyState.Die);
        }
        else
        {
            txt.text = Mathf.CeilToInt(info.leftHP).OptStr();
        }
    }
}