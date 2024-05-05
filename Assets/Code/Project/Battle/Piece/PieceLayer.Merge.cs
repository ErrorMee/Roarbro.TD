using System;
using System.Collections.Generic;
using UnityEngine;

public partial class PieceLayer : BattleLayer<PieceUnit>
{
    const int RemoveFrameMax = 24;
    int removeFrame = 0;
    const float RemoveStep = 1f / RemoveFrameMax;
    static Vector3 removeSpeed = new Vector3(RemoveStep, RemoveStep, RemoveStep);

    List<PieceUnit> removePieces = new List<PieceUnit>();

    List<PieceUnit> upgradePieces = new List<PieceUnit>();

    private void MergeEnter()
    {
        PieceModel.Instance.ExcuteMerge();

        removeFrame = 0;
        for (int i = 0; i < PieceModel.Instance.upgradePieces.Count; i++)
        {
            PieceUnit upgradeUnit = GetUnit(PieceModel.Instance.upgradePieces[i]);
            upgradeUnit.UpdateShow();
            upgradePieces.Add(upgradeUnit);
        }
        
        for (int i = 0; i < PieceModel.Instance.removePieces.Count; i++)
        {
            PieceInfo removeInfo = PieceModel.Instance.removePieces[i];
            removePieces.Add(GetUnit(removeInfo));
        }
    }

    private void MergeUpdate()
    {
        removeFrame++;
        if (removeFrame < RemoveFrameMax * 0.5f)
        {
            for (int i = 0; i < upgradePieces.Count; i++)
            {
                PieceUnit upgradeUnit = upgradePieces[i];
                upgradeUnit.transform.localScale += removeSpeed;
            }
        }
        else
        {
            for (int i = 0; i < upgradePieces.Count; i++)
            {
                PieceUnit upgradeUnit = upgradePieces[i];
                upgradeUnit.transform.localScale -= removeSpeed;
            }
        }

        bool playing = false;
        int upIndex = 0;
        foreach (PieceUnit removePiece in removePieces)
        {
            if (removePiece.info.RemoveMark == false)
            {
                playing = true;
                removePiece.transform.localScale -= removeSpeed;

                Vector3 crtPos = removePiece.transform.localPosition;
                Vector3 dir = upgradePieces[upIndex].transform.localPosition - crtPos;
                upIndex++;
                if (upIndex >= upgradePieces.Count)
                {
                    upIndex = 0;
                }

                removePiece.transform.localPosition += dir.normalized * removeSpeed.x;

                if (removePiece.transform.localScale.x <= 0.1f || 
                    removePiece.transform.localScale.z <= 0.1f)
                {
                    removePiece.info.RemoveMark = true;
                }
            }
        }
        if (playing == false && removeFrame >= RemoveFrameMax)
        {
            for (int i = 0; i < upgradePieces.Count; i++)
            {
                PieceUnit upgradeUnit = upgradePieces[i];
                upgradeUnit.transform.localScale = Vector3.one;
            }
            upgradePieces.Clear();
            if (removePieces.Count > 0)
            {
                ChangeState(PieceLayerState.Fill);
            }
            else
            {
                ChangeState(PieceLayerState.Move);
            }
        }
    }
}