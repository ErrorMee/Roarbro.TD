
using System;

public class BattleConfigs : Configs<BattleConfigs, BattleConfig>
{
}

[Serializable]
public class BattleConfig : Config
{
    public int terrain;

    public TerrainEnum terrainSelect = TerrainEnum.Ground;

    public TerrainEnum[] GetTerrains()
    {
        TerrainConfig terrainConfig = TerrainConfigs.Instance.GetConfigByID(terrain);
        if (terrainConfig != null)
        {
            return terrainConfig.terrains;
        }
        return null;
    }
}