using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : ConfigInfo<EnemyConfig>
{
    public Vector2Int index;
    public int level;

    public EnemyInfo(EnemyConfig config)
    {
        this.config = config;
    }
}