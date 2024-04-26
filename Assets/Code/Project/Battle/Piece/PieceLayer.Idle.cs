using System;
using System.Collections.Generic;
using UnityEngine;

public partial class PieceLayer : BattleLayer<PieceUnit>
{
    private void IdleEnter()
    {
        movePieces.Clear();
    }

    private void IdleUpdate()
    {
        if (InputModel.Instance.PressedThisFrame)
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

                ChangeState(PieceLayerState.Drag);
            }
            else
            {
                select.gameObject.SetActive(false);
                selectUnit = null;
            }
        }
    }
}