
public class TileUnitSelect : BattleUnit
{
    protected override void OnEnable()
    {
        meshRenderer.SetMPBColor(MatPropUtil.BaseColorKey, QualityConfigs.GetColor(QualityEnum.N), false);
        meshRenderer.SetMPBColor(MatPropUtil.AddColorKey, QualityConfigs.GetColor2(QualityEnum.N), false);
        meshRenderer.SetMPBInt(MatPropUtil.IndexKey, 1);
    }

    protected override void OnDisable()
    {
    }
}