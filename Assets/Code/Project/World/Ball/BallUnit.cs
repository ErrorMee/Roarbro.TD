using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class BallUnit : WorldUnit
{
    public FSM<BallState> fsm;

    [ReadOnlyProperty]
    public BallInfo info;

    public TextMeshPro txt;

    private void Awake()
    {
        fsm = new FSM<BallState>();
        fsm.AddState(BallState.Idle, null, null);
        fsm.AddState(BallState.Fight, FightEnter, FightUpdate);
        fsm.ChangeState(BallState.Idle);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        fsm.Update();
    }

    public void UpdateShow()
    {
        meshRenderer.SetMPBInt(MatPropUtil.IndexKey, info.config.id, false);

        Color color = Color.white;
        switch (info.config.id)
        {
            case 1:
                color = QualityConfigs.GetColor(QualityEnum.ER);
                break;
            case 2:
                color = QualityConfigs.GetColor(QualityEnum.UR);
                break;
            case 3:
                color = QualityConfigs.GetColor(QualityEnum.R);
                break;
            case 4:
                color = QualityConfigs.GetColor(QualityEnum.SR);
                break;
            case 5:
                color = QualityConfigs.GetColor(QualityEnum.SSR);
                break;
        }

        meshRenderer.SetMPBColor(MatPropUtil.BaseColorKey, color);
        if (info.level > 1)
        {
            if (info.level >= BallModel.BallMaxLV)
            {
                txt.text = "Max";
            }
            else
            {
                txt.text = info.level.OptStr();
            }
        }
        else
        {
            txt.text = String.Empty;
        }
    }
}