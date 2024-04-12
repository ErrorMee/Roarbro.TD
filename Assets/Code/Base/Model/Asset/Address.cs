using System.Collections.Generic;

public static class Address
{
    static readonly Dictionary<string, int> formatHashDic = new();
    static readonly Dictionary<int, Dictionary<string, string>> commonAddressDic = new();

    static string CommonAddress(string format, string assetName)
    {
        if (!formatHashDic.TryGetValue(format, out int formatHash))
        {
            formatHash = format.GetHashCode();
            formatHashDic.Add(format, formatHash);
        }

        if (!commonAddressDic.TryGetValue(formatHash, out Dictionary<string, string> groupDic))
        {
            groupDic = new Dictionary<string, string>();
            commonAddressDic.Add(formatHash, groupDic);
        }

        if (!groupDic.TryGetValue(assetName, out string address))
        {
            address = string.Format(format, assetName);
            groupDic.Add(assetName, address);
        }
        return address;
    }

    static readonly string UnitPrefabAddress = "Assets/Art/Unit/{0}.prefab";
    public static string UnitPrefab(string assetName)
    {
        return CommonAddress(UnitPrefabAddress, assetName);
    }

    static readonly string BgMaterialAddress = "Assets/Art/UI/BG/{0}.mat";
    public static string BgMaterial(string assetName)
    {
        return CommonAddress(BgMaterialAddress, assetName);
    }

    static readonly Dictionary<AudioGroupEnum, Dictionary<string, string>> audioAddressDic = new();
    public static string Audio(AudioGroupEnum group, string assetName)
    {
        if (!audioAddressDic.TryGetValue(group, out Dictionary<string, string> groupDic))
        {
            groupDic = new Dictionary<string, string>();
            audioAddressDic.Add(group, groupDic);
        }

        if (!groupDic.TryGetValue(assetName, out string address))
        {
            address = string.Format("Assets/Art/Audio/{0}/{1}.wav", group.ToString(), assetName);
            groupDic.Add(assetName, address);
        }

        return address;
    }
}