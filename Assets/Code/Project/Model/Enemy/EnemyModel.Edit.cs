using System;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyModel : Singleton<EnemyModel>, IDestroy
{
    public EnemyTemplate crtTemplate;

    public EnemyTemplate[] enemyTemplates;

    public void EditInit()
    {
        enemyTemplates = new EnemyTemplate[EnemyConfigs.All.Length];
        for (int i = 0; i < EnemyConfigs.All.Length; i++)
        {
            EnemyConfig enemyConfig = EnemyConfigs.All[i];
            EnemyTemplate enemyTemplate = new EnemyTemplate();
            enemyTemplate.enemyID = enemyConfig.id;
            enemyTemplate.level = 1;
            enemyTemplates[i] = enemyTemplate;
        }
    }

    public void SelectTemplate(int index)
    {
        crtTemplate = enemyTemplates[index];
    }
}