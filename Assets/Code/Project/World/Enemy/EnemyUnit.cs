using System;
using TMPro;
using UnityEngine;

public partial class EnemyUnit : WorldUnit
{
    public FSM<EnemyState> fsm;

    [ReadOnlyProperty]
    public EnemyInfo info;
    public TextMeshPro txt;

    private void Awake()
    {
        fsm = new FSM<EnemyState>();
        fsm.AddState(EnemyState.Idle, IdleEnter, IdleUpdate);
        fsm.AddState(EnemyState.Fight, FightEnter, FightUpdate);
        fsm.AddState(EnemyState.Die, DieEnter, DieUpdate);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        fsm.Update();
    }

    public void UpdateShow()
    {
        meshRenderer.SetMPBInt(MatPropUtil.IndexKey, info.config.avatar, false);
        meshRenderer.SetMPBColor(MatPropUtil.BaseColorKey, info.config.color);

        if (info.enemyTemplate.Exist())
        {
            txt.text = Mathf.CeilToInt(info.leftHP).OptStr();
        }
        else
        {
            txt.text = string.Empty;
        }
    }
}