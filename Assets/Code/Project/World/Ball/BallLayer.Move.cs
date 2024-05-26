using System;
using System.Collections.Generic;
using UnityEngine;

public partial class BallLayer : WorldLayer<BallUnit>
{
    const float moveSpeed = 0.12f;
    const float moveStreshold = moveSpeed * moveSpeed;

    HashSet<BallUnit> moveBalls = new HashSet<BallUnit>();
    HashSet<BallUnit> moveEndBalls = new HashSet<BallUnit>();

    private void MoveEnter()
    {
        CreateSelect();
        if (moveBalls.Count > 1)
        {
            select.gameObject.SetActive(false);
        }
    }

    private void OnAddMoveBall(object obj = null)
    {
        BallInfo moveInfo = (BallInfo)obj;
        BallUnit moveUnit = GetUnit(moveInfo);
        moveBalls.Add(moveUnit);
    }

    private void MoveUpdate()
    {
        moveEndBalls.Clear();

        foreach (BallUnit moveUnit in moveBalls)
        {
            Vector3 targetPos = new Vector3(moveUnit.info.GetViewX(), 0, moveUnit.info.GetViewZ());

            Vector3 crtPos = moveUnit.transform.localPosition;

            Vector3 dir = targetPos - crtPos;
            if (dir.sqrMagnitude <= moveStreshold)
            {
                moveUnit.transform.localPosition = targetPos;
                moveEndBalls.Add(moveUnit);

                units[moveUnit.info.index.x, moveUnit.info.index.y] = moveUnit;
            }
            else
            {
                moveUnit.transform.localPosition += dir.normalized * moveSpeed;
                moveUnit.transform.localPosition = new Vector3(moveUnit.transform.localPosition.x,
                    0.02f, moveUnit.transform.localPosition.z);
            }
        }

        foreach (BallUnit moveEndUnit in moveEndBalls)
        {
            moveBalls.Remove(moveEndUnit);
        }

        if (moveBalls.Count < 1)
        {
            BallModel.Instance.CheckMerges();
            if (BallModel.Instance.readyMerges.Count > 2)
            {
                fsm.ChangeState(BallLayerState.Merge);
            }
            else
            {
                fsm.ChangeState(BallLayerState.Idle);
            }
        }
    }
}