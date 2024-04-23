
using System;

public class BattleConfigs : Configs<BattleConfigs, BattleConfig>
{
}

[Serializable]
public class BattleConfig : Config
{
    public int terrain;

    public TerrainEnum terrainSelect = TerrainEnum.Water;

    public TerrainConfig GetTerrainConfig()
    {
        TerrainConfig terrainConfig = TerrainConfigs.Instance.GetConfigByID(terrain);
        if (terrainConfig != null)
        {
            return terrainConfig;
        }
        return null;
    }

    public TerrainEnum[] GetTerrains()
    {
        TerrainConfig terrainConfig = GetTerrainConfig();
        if (terrainConfig != null)
        {
            return terrainConfig.terrains;
        }
        return null;
    }
}