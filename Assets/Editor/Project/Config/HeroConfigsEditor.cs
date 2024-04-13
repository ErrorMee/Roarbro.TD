using UnityEditor;

[CustomEditor(typeof(HeroConfigs))]
public class HeroConfigsEditor : ConfigsEditor<HeroConfig, HeroConfigs>
{
    [MenuItem(ConfigMenu + "Hero")]
    static void Create()
    {
        CreateConfigs<HeroConfigs>("Hero");
    }

    protected override void DrawHead()
    {
        base.DrawHead();
        //TODO
    }

    protected override void DrawMenuAdd()
    {
        base.DrawMenuAdd();
        //TODO
    }

    protected override void DrawConfig(int index, HeroConfig config)
    {
        base.DrawConfig(index, config);
        //TODO
    }
}