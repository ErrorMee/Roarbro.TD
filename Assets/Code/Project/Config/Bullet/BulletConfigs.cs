using System;

public class BulletConfigs : Configs<BulletConfigs, BulletConfig>
{
    
}

[Serializable]
public class BulletConfig : Config
{
    public float radius;
    public float speed;
    public float life;
}
