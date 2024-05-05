using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLayer : BattleLayer<EnemyUnit>
{
    public EnemyUnit[,] units;

    protected override void Awake()
    {
        base.Awake();
        units = new EnemyUnit[GridUtil.XCount, GridUtil.YCount];

    }

    private EnemyUnit GetUnit(EnemyInfo info)
    {
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                EnemyUnit unit = units[x, y];
                if (unit.info == info)
                {
                    return unit;
                }
            }
        }
        return null;
    }
}