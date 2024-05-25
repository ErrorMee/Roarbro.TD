using System;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyModel : Singleton<EnemyModel>, IDestroy
{
    public EnemyInfo[,] infos;

    List<EnemyInfo> leftEnemys = new List<EnemyInfo>();

    public EnemyModel Init(BattleInfo battleInfo)
    {
        if (battleInfo.edit)
        {
            EditInit();
        }
        
        infos = new EnemyInfo[GridUtil.XCount, GridUtil.YCount];
        
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                EnemyTemplate enemyInfoConfig = BattleModel.Instance.battle.army.GetEnemyInfoConfig(x, y);
                EnemyInfo info = new EnemyInfo(enemyInfoConfig);
                infos[x, y] = info;
                info.index = new Vector2Int(x, y);
                if (enemyInfoConfig.enemyID > 0)
                {
                    leftEnemys.Add(info);
                }
            }
        }
        return Instance;
    }

    public void EnemyDie(EnemyInfo enemy)
    {
        leftEnemys.Remove(enemy);
        if (leftEnemys.Count == 0)
        {
            BattleModel.Instance.Complete();
            WindowModel.Open(WindowEnum.Win);
        }
    }
}