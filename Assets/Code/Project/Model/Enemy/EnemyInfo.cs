using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : ConfigInfo<EnemyConfig>
{
    public EnemyTemplate enemyInfoConfig;
    public Vector2Int index;

    public EnemyInfo(EnemyTemplate enemyInfoConfig)
    {
        SetEnemyInfoConfig(enemyInfoConfig);
    }

    public void SetEnemyInfoConfig(EnemyTemplate enemyInfoConfig)
    {
        this.enemyInfoConfig = enemyInfoConfig;
        EnemyConfig enemyConfig = EnemyConfigs.Instance.GetConfigByID(enemyInfoConfig.enemyID);
        config = enemyConfig;
    }
}