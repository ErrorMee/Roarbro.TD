using System;
using System.Collections.Generic;
using UnityEngine;

public partial class PieceLayer : BattleLayer<PieceUnit>
{
    const int RemoveFrameCount = 24;
    int removeFrame = 0;
    const float RemoveStep = 1f / RemoveFrameCount;
    static Vector3 removeSpeed = new Vector3(RemoveStep, RemoveStep, RemoveStep);

    List<PieceUnit> removePieces = new List<PieceUnit>();

    PieceUnit upgradeUnit;

    private void MergeEnter()
    {
        PieceModel.Instance.ExcuteMerge();

        removeFrame = 0;
        upgradeUnit = GetPieceUnit(PieceModel.Instance.upgradePiece);
        upgradeUnit.UpdateShow();

        for (int i = 0; i < PieceModel.Instance.removePieces.Count; i++)
        {
            PieceInfo removeInfo = PieceModel.Instance.removePieces[i];
            removePieces.Add(GetPieceUnit(removeInfo));
        }
    }

    private void MergeUpdate()
    {
        removeFrame++;
        if (removeFrame < RemoveFrameCount * 0.5f)
        {
            upgradeUnit.transform.localScale += removeSpeed;
        }
        else
        {
            upgradeUnit.transform.localScale -= removeSpeed;
        }

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
            upgradeUnit.transform.localScale = Vector3.one;
            ChangeState(PieceLayerState.Fill);
        }
    }
}