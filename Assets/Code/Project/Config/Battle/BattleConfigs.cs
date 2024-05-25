
using System;

public class BattleConfigs : Configs<BattleConfigs, BattleConfig>
{
}

[Serializable]
public class BattleConfig : Config
{
    public int terrain;

    public TerrainEnum terrainSelect = TerrainEnum.Water;

    public int steps = 12;
}