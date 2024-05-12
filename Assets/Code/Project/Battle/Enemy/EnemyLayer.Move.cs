using System;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyLayer : WorldLayer<EnemyUnit>
{
    private float moveSpeed = 0.01f;

    private void MoveEnter()
    {
        
    }

    private void MoveUpdate()
    {
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                EnemyUnit unit = units[x, y];
                if (unit.info.config.id > 0)
                {
                    unit.transform.localPosition += new Vector3(0, 0, -moveSpeed);
                }
            }
        }
    }
}