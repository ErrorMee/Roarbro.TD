using System;

public class WindowConfigs : Configs<WindowConfigs, WindowConfig>
{
}

[Serializable]
public class WindowConfig : Config
{
    public WindowLayerEnum layer;

    public WindowShowEnum show;

    public WindowEnum[] depends;

    public BGEnum bg = BGEnum.None;

    public string Address
    {
        get{ return string.Format("Assets/Art/Window/{0}Window/{0}Window.prefab", ((WindowEnum)id).ToString()); }     
    }
}