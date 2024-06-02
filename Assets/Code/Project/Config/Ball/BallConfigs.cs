
using System;
using UnityEngine;

public class BallConfigs : Configs<BallConfigs, BallConfig>
{
    
}

[Serializable]
public class BallConfig : Config
{
    public Color color;
    public int attack;
    public float attCD;
    public float attRadius;
}
