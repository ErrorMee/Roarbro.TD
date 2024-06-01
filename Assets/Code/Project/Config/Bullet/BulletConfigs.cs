
using System;
using UnityEngine;

public class BulletConfigs : Configs<BulletConfigs, BulletConfig>
{
    
}

[Serializable]
public class BulletConfig : Config
{
    public float size;
    public Color color;
    public float speed;
}
