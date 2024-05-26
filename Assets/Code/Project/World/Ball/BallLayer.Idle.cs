using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public partial class BallLayer : WorldLayer<BallUnit>
{
    private void IdleEnter()
    {
        CreateSelect();

        moveBalls.Clear();
        if (BallModel.Instance.LeftStep <= 0)
        {
            BattleModel.Instance.CreateLayer(typeof(EnemyLayer));
            fsm.ChangeState(BallLayerState.Fight);
        }
    }

    private void IdleUpdate()
    {
        if (BallModel.Instance.LeftStep > 0 && InputModel.Instance.PressedThisFrame)
        {
            Vector3 worldPos = CameraModel.Instance.ScreenToWorldPos(InputModel.Instance.Touch0LastPos,
            transform.position.y);
            worldPos = GridUtil.WorldToGridPos(worldPos, false);
            Vector2Int index = GridUtil.WorldToGridIndex(worldPos);

            select.transform.localPosition = worldPos;
            
            if (GridUtil.InGrid(index.x, index.y))
            {
                select.gameObject.SetActive(true);

                selectUnit = units[index.x, index.y];

                fsm.ChangeState(BallLayerState.Drag);
            }
            else
            {
                select.gameObject.SetActive(false);
                selectUnit = null;
            }
        }
    }
}