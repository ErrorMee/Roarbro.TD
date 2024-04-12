
using System;
using UnityEngine;

public class QualityConfigs : Configs<QualityConfigs, QualityConfig>
{
    public static Color GetColor(int id)
    {
        foreach (QualityConfig config in Instance.all)
        {
            if (config.id == id)
            {
                return config.color1;
            }
        }
        return Color.white;
    }

    public static Color GetColor(QualityEnum quality)
    {
        foreach (QualityConfig config in Instance.all)
        {
            if (config.quality == quality)
            {
                return config.color1;
            }
        }
        return Color.white;
    }

    public static Color GetColor2(QualityEnum quality)
    {
        foreach (QualityConfig config in Instance.all)
        {
            if (config.quality == quality)
            {
                return config.color2;
            }
        }
        return Color.white;
    }
}

[Serializable]
public class QualityConfig : Config
{
    public QualityEnum quality;
    public Color color1;
    public Color color2;
}
