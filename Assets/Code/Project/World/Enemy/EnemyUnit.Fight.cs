using UnityEngine;

public partial class EnemyUnit : MonoBehaviour
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
        int attackValue = ballInfo.GetAttackValue();
        info.leftHP -= attackValue;
        if (info.leftHP <= 0)
        {
            fsm.ChangeState(EnemyState.Die);
        }
        else
        {
            txt.text = Mathf.CeilToInt(info.leftHP).OptStr();
        }

        BubbleInfo bubbleInfo = SharedPool<BubbleInfo>.Get();
        bubbleInfo.x = transform.localPosition.x;
        bubbleInfo.z = transform.localPosition.z;
        bubbleInfo.value = attackValue;
        EventModel.Send(EventEnum.BubbleUnit, bubbleInfo);
    }
}