
public class TileUnitSelect : BattleUnit
{
    protected override void OnEnable()
    {
        meshRenderer.SetMPBColor(MatPropUtil.BaseColorKey, QualityConfigs.GetColor(QualityEnum.N), false);
    }

    protected override void OnDisable()
    {
    }
}