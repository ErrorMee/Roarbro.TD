using UnityEngine;

public enum TerrainEnum : byte
{
    /// <summary>
    /// Hold[0]
    /// </summary>
    Water = 0b00,
    /// <summary>
    /// Hold[1]
    /// </summary>
    Land = 0b01,
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
    public static Color GetColor(this TerrainEnum terrain)
    {
        switch (terrain)
        {
            case TerrainEnum.Water:
                return QualityConfigs.GetColor(QualityEnum.SR);
            case TerrainEnum.Land:
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
            case TerrainEnum.Land:
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