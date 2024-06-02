using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class BallUnit : MonoBehaviour
{
    public FSM<BallState> fsm;

    [ReadOnlyProperty]
    public BallInfo info;

    public MeshRenderer meshRenderer;
    public TextMeshPro txt;

    private void Awake()
    {
        fsm = new FSM<BallState>();
        fsm.AddState(BallState.Idle, null, null);
        fsm.AddState(BallState.Fight, FightEnter, FightUpdate);
        fsm.ChangeState(BallState.Idle);
    }

    void FixedUpdate()
    {
        if (BattleModel.Instance.pause == false)
        {
            fsm.Update();
        }
    }

    public void UpdateShow()
    {
        meshRenderer.SetMPBInt(MatPropUtil.IndexKey, info.config.id, false);

        meshRenderer.SetMPBColor(MatPropUtil.BaseColorKey, info.config.color);
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