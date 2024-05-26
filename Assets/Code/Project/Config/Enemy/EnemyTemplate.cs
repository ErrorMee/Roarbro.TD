using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyTemplate
{
    public int enemyID = 0;
    public int level = 1;

    public bool Exist()
    {
        return enemyID > 0 && level > 0;
    }

    public bool Equal(EnemyTemplate template)
    {
        return template.enemyID == enemyID && template.level == level;
    }
}