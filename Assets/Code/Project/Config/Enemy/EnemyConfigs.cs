
using System;
using UnityEngine;

public class EnemyConfigs : Configs<EnemyConfigs, EnemyConfig>
{
    
}

[Serializable]
public class EnemyConfig : Config
{
    public int avatar;
    public Color color;

    public float speed = 1;
}
