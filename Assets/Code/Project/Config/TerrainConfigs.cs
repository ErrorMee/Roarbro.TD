using System;

public class TerrainConfigs : Configs<TerrainConfigs, TerrainConfig>
{
    
}

[Serializable]
public class TerrainConfig : Config
{
    public TerrainEnum[] terrains;
}
