using System;
using System.Collections.Generic;
using UnityEngine;

public partial class PieceLayer : BattleLayer<PieceUnit>
{
    const float moveSpeed = 0.1f;
    const float moveStreshold = moveSpeed * moveSpeed;

    HashSet<PieceUnit> movePieces = new HashSet<PieceUnit>();
    HashSet<PieceUnit> moveEndPieces = new HashSet<PieceUnit>();

    private void MoveEnter()
    {
    }

    private void MoveExit()
    {
    }

    private void OnAddMovePiece(object obj = null)
    {
        PieceInfo moveInfo = (PieceInfo)obj;
        PieceUnit moveUnit = GetPieceUnit(moveInfo);
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
            }
        }

        foreach (PieceUnit moveEndUnit in moveEndPieces)
        {
            movePieces.Remove(moveEndUnit);
        }

        if (movePieces.Count < 1)
        {
            ChangeState(PieceLayerState.Idle);
        }
    }
}