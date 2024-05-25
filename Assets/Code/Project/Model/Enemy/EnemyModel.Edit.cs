using System;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyModel : Singleton<EnemyModel>, IDestroy
{
    public EnemyTemplate crtTemplate;

    EnemyTemplate[] enemyTemplates;

    public void EditInit()
    {
        enemyTemplates = new EnemyTemplate[EnemyConfigs.All.Length];
        for (int i = 0; i < EnemyConfigs.All.Length; i++)
        {
            EnemyConfig enemyConfig = EnemyConfigs.All[i];
            EnemyTemplate enemyTemplate = new EnemyTemplate();
            enemyTemplate.enemyID = enemyConfig.id;
            enemyTemplate.level = 0;
            enemyTemplates[i] = enemyTemplate;
        }
        SelectTemplate(0);
    }

    public void SelectTemplate(int index)
    {
        crtTemplate = enemyTemplates[index];
    }
}