using System;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyLayer : WorldLayer<EnemyUnit>
{
    public static FSM<EnemyLayerState> fsm = new FSM<EnemyLayerState>();

    public EnemyUnit[,] units;

    protected override void Awake()
    {
        base.Awake();

        Init();
        UpdateUnits();
    }

    private void Init()
    {
        fsm.mStates.Add(new State<EnemyLayerState>(EnemyLayerState.Edit, EditEnter, EditUpdate));
        fsm.mStates.Add(new State<EnemyLayerState>(EnemyLayerState.Idle, IdleEnter, IdleUpdate));

        units = new EnemyUnit[GridUtil.XCount, GridUtil.YCount];
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                EnemyUnit unit = CreateUnit();
                units[x, y] = unit;
                unit.info = EnemyModel.Instance.infos[x, y];
            }
        }
    }

    private void UpdateUnits()
    {
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                EnemyUnit unit = units[x, y];
                unit.UpdateShow();
                unit.transform.localPosition = new Vector3(x - GridUtil.XRadiusCount, 0, y - GridUtil.YRadiusCount);
            }
        }
    }

    private void Update()
    {
        fsm.Update();
    }
}