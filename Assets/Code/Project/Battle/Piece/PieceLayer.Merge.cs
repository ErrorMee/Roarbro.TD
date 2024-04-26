using System;
using System.Collections.Generic;
using UnityEngine;

public partial class PieceLayer : BattleLayer<PieceUnit>
{
    static Vector3 removeSpeed = new Vector3(0.05f, 0.05f, 0.05f);

    List<PieceUnit> removePieces = new List<PieceUnit>();

    PieceUnit upgradeUnit;

    private void MergeEnter()
    {
        PieceModel.Instance.ExcuteMerge();

        upgradeUnit = GetPieceUnit(PieceModel.Instance.upgradePiece);
        upgradeUnit.UpdateShow();

        for (int i = 0; i < PieceModel.Instance.removePieces.Count; i++)
        {
            PieceInfo removeInfo = PieceModel.Instance.removePieces[i];
            removePieces.Add(GetPieceUnit(removeInfo));
        }
    }

    private void MergeExit()
    {
    }

    private void MergeUpdate()
    {
        bool playing = false;
        foreach (PieceUnit removePiece in removePieces)
        {
            if (removePiece.info.RremoveMark == false)
            {
                playing = true;
                removePiece.transform.localScale -= removeSpeed;

                Vector3 crtPos = removePiece.transform.localPosition;
                Vector3 dir = upgradeUnit.transform.localPosition - crtPos;

                removePiece.transform.localPosition += dir.normalized * removeSpeed.x;

                if (removePiece.transform.localScale.x <= 0.1f || 
                    removePiece.transform.localScale.z <= 0.1f)
                {
                    removePiece.info.RremoveMark = true;
                }
            }
        }
        if (playing == false)
        {
            ChangeState(PieceLayerState.Fill);
        }
    }
}