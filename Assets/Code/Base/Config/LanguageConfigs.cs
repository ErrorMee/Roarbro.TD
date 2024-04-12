
using System;
using UnityEngine;

public class LanguageConfigs : Configs<LanguageConfigs, LanguageConfig>
{
    public LanguageType[] types;
}

[Serializable]
public class LanguageType
{
    public SystemLanguage type;
    public string tag;
    public string code;
}


[Serializable]
public class LanguageConfig : Config
{
    public string desc;

    public string[] values = new string[] { };

    public string GetLanguage(int idx)
    {
        return values[idx];
    }
}