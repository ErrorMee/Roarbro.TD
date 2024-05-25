
using System;

public class ProjectConfigs : Configs<ProjectConfigs, ProjectConfig>
{
    public const string company = "♦Roarbro";
    public static bool isQuiting = false;

    public int fps = 60;
    public bool analyzer = true;
    public bool netLog;
    public string HttpHost = "Roarbro";
    public string LoginURL;
}

[Serializable]
public class ProjectConfig : Config
{
    
}
