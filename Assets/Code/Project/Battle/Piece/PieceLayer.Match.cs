using System;
using System.Collections.Generic;
using UnityEngine;

public partial class PieceLayer : BattleLayer<PieceUnit>
{
    static Vector3 removeSpeed = new Vector3(0.05f, 0.05f, 0.05f);

    List<PieceUnit> removePieces = new List<PieceUnit>();

    private void MatchEnter()
    {
        PieceModel.Instance.ExcuteMatch();

        for (int i = 0; i < PieceModel.Instance.upgradePieces.Count; i++)
        {
            PieceInfo upgradeInfo = PieceModel.Instance.upgradePieces[i];
            PieceUnit upgradeUnit = GetPieceUnit(upgradeInfo);
            upgradeUnit.UpdateShow();
        }
        removePieces.Clear();
        for (int i = 0; i < PieceModel.Instance.removePieces.Count; i++)
        {
            PieceInfo removeInfo = PieceModel.Instance.removePieces[i];
            removePieces.Add(GetPieceUnit(removeInfo));
        }
    }

    private void MatchExit()
    {
    }

    private void MatchUpdate()
    {
        bool playing = false;
        foreach (PieceUnit removePiece in removePieces)
        {
            if (removePiece.info.level >= 0)
            {
                playing = true;
                removePiece.transform.localScale -= removeSpeed;
                if (removePiece.transform.localScale.x <= 0.1f || 
                    removePiece.transform.localScale.z <= 0.1f)
                {
                    removePiece.info.level = -1;
                }
            }
        }
    }
}