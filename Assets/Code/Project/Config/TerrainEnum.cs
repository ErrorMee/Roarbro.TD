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
}

public static class TerrainEnumExt
{
    public static string GetName(this TerrainEnum terrain)
    {
        return LanguageModel.Get(10051 + (byte)terrain);
    }
}