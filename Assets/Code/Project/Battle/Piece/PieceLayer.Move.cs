using System;
using System.Collections.Generic;
using UnityEngine;

public partial class PieceLayer : BattleLayer<PieceUnit>
{
    const float moveSpeed = 0.12f;
    const float moveStreshold = moveSpeed * moveSpeed;

    HashSet<PieceUnit> movePieces = new HashSet<PieceUnit>();
    HashSet<PieceUnit> moveEndPieces = new HashSet<PieceUnit>();

    private void MoveEnter()
    {
        if (movePieces.Count > 1)
        {
            select.gameObject.SetActive(false);
        }
    }

    private void OnAddMovePiece(object obj = null)
    {
        PieceInfo moveInfo = (PieceInfo)obj;
        PieceUnit moveUnit = GetUnit(moveInfo);
        movePieces.Add(moveUnit);
    }

    private void MoveUpdate()
    {
        moveEndPieces.Clear();

        foreach (PieceUnit moveUnit in movePieces)
        {
            Vector3 targetPos = new Vector3(moveUnit.info.GetViewX(), 0, moveUnit.info.GetViewZ());

            Vector3 crtPos = moveUnit.transform.localPosition;

            Vector3 dir = targetPos - crtPos;
            if (dir.sqrMagnitude <= moveStreshold)
            {
                moveUnit.transform.localPosition = targetPos;
                moveEndPieces.Add(moveUnit);

                units[moveUnit.info.index.x, moveUnit.info.index.y] = moveUnit;
            }
            else
            {
                moveUnit.transform.localPosition += dir.normalized * moveSpeed;
                moveUnit.transform.localPosition = new Vector3(moveUnit.transform.localPosition.x,
                    0.02f, moveUnit.transform.localPosition.z);
            }
        }

        foreach (PieceUnit moveEndUnit in moveEndPieces)
        {
            movePieces.Remove(moveEndUnit);
        }

        if (movePieces.Count < 1)
        {
            PieceModel.Instance.CheckMerges();
            if (PieceModel.Instance.readyMerges.Count > 2)
            {
                ChangeState(PieceLayerState.Merge);
            }
            else
            {
                ChangeState(PieceLayerState.Idle);
            }
        }
    }
}