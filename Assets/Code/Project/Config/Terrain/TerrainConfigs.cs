using System;
using UnityEngine;

public class TerrainConfigs : Configs<TerrainConfigs, TerrainConfig>
{
    
}

[Serializable]
public class TerrainConfig : Config
{
    public Color[] colors;

    public TerrainEnum[] terrains;

    public TerrainEnum GetTerrain(int posx, int posy)
    {
        return terrains[GridUtil.GetIndex(posx, posy)];
    }

    public Color GetColor(TerrainEnum terrain)
    {
        return colors[(byte)terrain];
    }
}
