using System;
using System.Collections.Generic;
using UnityEngine;

public partial class BallLayer : WorldLayer<BallUnit>
{
    const int RemoveFrameMax = 24;
    int removeFrame = 0;
    const float RemoveStep = 1f / RemoveFrameMax;
    static Vector3 removeSpeed = new Vector3(RemoveStep, RemoveStep, RemoveStep);

    List<BallUnit> removeBalls = new List<BallUnit>();

    List<BallUnit> upgradeBalls = new List<BallUnit>();

    private void MergeEnter()
    {
        BallModel.Instance.ExcuteMerge();

        removeFrame = 0;
        for (int i = 0; i < BallModel.Instance.upgradeBalls.Count; i++)
        {
            BallUnit upgradeUnit = GetUnit(BallModel.Instance.upgradeBalls[i]);
            upgradeUnit.UpdateShow();
            upgradeBalls.Add(upgradeUnit);
        }
        
        for (int i = 0; i < BallModel.Instance.removeBalls.Count; i++)
        {
            BallInfo removeInfo = BallModel.Instance.removeBalls[i];
            removeBalls.Add(GetUnit(removeInfo));
        }
    }

    private void MergeUpdate()
    {
        removeFrame++;
        if (removeFrame < RemoveFrameMax * 0.5f)
        {
            for (int i = 0; i < upgradeBalls.Count; i++)
            {
                BallUnit upgradeUnit = upgradeBalls[i];
                upgradeUnit.transform.localScale += removeSpeed;
            }
        }
        else
        {
            for (int i = 0; i < upgradeBalls.Count; i++)
            {
                BallUnit upgradeUnit = upgradeBalls[i];
                upgradeUnit.transform.localScale -= removeSpeed;
            }
        }

        bool playing = false;
        int upIndex = 0;
        foreach (BallUnit removeBall in removeBalls)
        {
            if (removeBall.info.RemoveMark == false)
            {
                playing = true;
                removeBall.transform.localScale -= removeSpeed;

                Vector3 crtPos = removeBall.transform.localPosition;
                Vector3 dir = upgradeBalls[upIndex].transform.localPosition - crtPos;
                upIndex++;
                if (upIndex >= upgradeBalls.Count)
                {
                    upIndex = 0;
                }

                removeBall.transform.localPosition += dir.normalized * removeSpeed.x;

                if (removeBall.transform.localScale.x <= 0.1f || 
                    removeBall.transform.localScale.z <= 0.1f)
                {
                    removeBall.info.RemoveMark = true;
                }
            }
        }
        if (playing == false && removeFrame >= RemoveFrameMax)
        {
            for (int i = 0; i < upgradeBalls.Count; i++)
            {
                BallUnit upgradeUnit = upgradeBalls[i];
                upgradeUnit.transform.localScale = Vector3.one;
            }
            upgradeBalls.Clear();
            if (removeBalls.Count > 0)
            {
                fsm.ChangeState(BallLayerState.Fill);
            }
            else
            {
                fsm.ChangeState(BallLayerState.Move);
            }
        }
    }
}