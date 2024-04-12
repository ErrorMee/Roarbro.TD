
using UnityEngine;
/// <summary>
/// Walkable|Placeable
/// </summary>
public enum TerrainEnum : byte
{
    /// <summary>
    /// Walkable[0]，Placeable[0]
    /// </summary>
    Water = 0b00,
    /// <summary>
    /// Walkable[0]，Placeable[1]
    /// </summary>
    Grass = 0b01,
    /// <summary>
    /// Walkable[1]，Placeable[0]
    /// </summary>
    Road = 0b10,
    /// <summary>
    /// Walkable[1]，Placeable[1]
    /// </summary>
    Ground = 0b11,
}

public static class TerrainEnumExt
{
    public static TerrainEnum Prev(this TerrainEnum terrain)
    {
        return (byte)terrain == 0 ? (TerrainEnum)0b11 : (terrain - 1);
    }

    public static TerrainEnum Next(this TerrainEnum terrain)
    {
        return (byte)terrain == 0b11 ? 0 : (terrain + 1);
    }

    public static bool Walkable(this TerrainEnum terrain)
    {
        return ((byte)terrain & 0b10) > 0;
    }

    public static bool Placeable(this TerrainEnum terrain)
    {
        return ((byte)terrain & 0b01) > 0;
    }

    public static Color GetColor(this TerrainEnum terrain)
    {
        switch (terrain)
        {
            case TerrainEnum.Water:
                return QualityConfigs.GetColor(QualityEnum.SR);
            case TerrainEnum.Grass:
                return QualityConfigs.GetColor(QualityEnum.R);
            default:
                return QualityConfigs.GetColor2(QualityEnum.R);
        }
    }

    public static Color GetConfigColor(this TerrainEnum terrain)
    {
        switch (terrain)
        {
            case TerrainEnum.Water:
                return QualityConfigs.GetColor(QualityEnum.SR);
            case TerrainEnum.Grass:
                return QualityConfigs.GetColor(QualityEnum.R);
            case TerrainEnum.Ground:
                return QualityConfigs.GetColor2(QualityEnum.SSR);
            default:
                return QualityConfigs.GetColor2(QualityEnum.R);
        }
    }

    public static string GetName(this TerrainEnum terrain)
    {
        return LanguageModel.Get(10051 + (byte)terrain);
    }
}