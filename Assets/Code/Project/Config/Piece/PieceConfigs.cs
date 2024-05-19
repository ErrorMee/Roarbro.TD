
using System;

public class PieceConfigs : Configs<PieceConfigs, PieceConfig>
{
    
}

[Serializable]
public class PieceConfig : Config
{
    public int attack;
    public float attCD;
    public float attRadius;
}
