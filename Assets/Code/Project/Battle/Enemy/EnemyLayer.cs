using System;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyLayer : BattleLayer<EnemyUnit>
{
    public EnemyUnit[,] units;

    protected override void Awake()
    {
        base.Awake();

        Init();
        UpdateUnits();
        CreateSelect();
    }

    private void Init()
    {
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

    private void UpdateUnits(object obj = null)
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
}