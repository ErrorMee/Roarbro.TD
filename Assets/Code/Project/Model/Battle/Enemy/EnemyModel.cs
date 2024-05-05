using System;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyModel : Singleton<EnemyModel>, IDestroy
{
    public EnemyInfo[,] infos;

    public EnemyModel Init()
    {
        infos = new EnemyInfo[GridUtil.XCount, GridUtil.YCount];
        EnemyConfig emptyEnemy = EnemyConfigs.ConfigByID(0);
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                EnemyInfo info = new EnemyInfo(emptyEnemy);

                infos[x, y] = info;
                info.level = 0;
                info.index = new Vector2Int(x, y);
            }
        }
        return Instance;
    }
}