using System;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyLayer : WorldLayer<EnemyUnit>
{
    private void ReadyEnter()
    {
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                EnemyUnit unit = units[x, y];
                unit.UpdateShow();
                unit.transform.localPosition = new Vector3(x - GridUtil.XRadiusCount, 0, y);
            }
        }
    }

    private void ReadyUpdate()
    {
        
    }
}