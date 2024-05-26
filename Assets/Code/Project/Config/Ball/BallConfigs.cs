
using System;

public class BallConfigs : Configs<BallConfigs, BallConfig>
{
    
}

[Serializable]
public class BallConfig : Config
{
    public int attack;
    public float attCD;
    public float attRadius;
}
