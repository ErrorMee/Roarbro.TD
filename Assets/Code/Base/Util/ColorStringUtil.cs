using UnityEngine;

public static class ColorStringUtil
{
	public static string ColorString(Color color, string source, bool useAlpha = false)
    {
        string hex;
        if (useAlpha)
        {
            hex = ColorUtility.ToHtmlStringRGBA(color);
        }
        else
        {
            hex = ColorUtility.ToHtmlStringRGB(color);
        }

        string colorString = string.Format("<color=#{0}>{1}</color>", hex, source);
        return colorString;
    }

	public static string ColorString(string hex, string source)
	{
        //有效性验证
        _ = Color.white;
        if (!ColorUtility.TryParseHtmlString($"#{hex}", out _))
        {
            hex = "FFFFFF";
        }

        string colorString = string.Format("<color=#{0}>{1}</color>", hex, source);
		return colorString;
	}
}