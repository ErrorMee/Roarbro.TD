using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : ConfigInfo<EnemyConfig>
{
    public EnemyInfoConfig enemyInfoConfig;
    public Vector2Int index;

    public EnemyInfo(EnemyInfoConfig enemyInfoConfig)
    {
        SetEnemyInfoConfig(enemyInfoConfig);
    }

    public void SetEnemyInfoConfig(EnemyInfoConfig enemyInfoConfig)
    {
        this.enemyInfoConfig = enemyInfoConfig;
        EnemyConfig enemyConfig = EnemyConfigs.Instance.GetConfigByID(enemyInfoConfig.enemyID);
        config = enemyConfig;
    }
}