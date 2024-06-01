using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : ConfigInfo<EnemyConfig>
{
    public EnemyTemplate enemyTemplate;
    public Vector2Int index;

    public float leftHP;

    public EnemyInfo(EnemyTemplate enemyTemplate)
    {
        SetEnemyTemplate(enemyTemplate);
        leftHP = GetMaxHP();
    }

    public void SetEnemyTemplate(EnemyTemplate enemyTemplate)
    {
        this.enemyTemplate = enemyTemplate;
        EnemyConfig enemyConfig = EnemyConfigs.Instance.GetConfigByID(enemyTemplate.enemyID);
        config = enemyConfig;
    }

    public float GetMaxHP()
    {
        return config.hp * enemyTemplate.level;
    }
}