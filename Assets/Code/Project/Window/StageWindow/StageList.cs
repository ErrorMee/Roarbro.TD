public class StageList : ScrollList<BattleInfo, StageCell>
{
    class CellGroup : DefaultCellGroup { }

    protected override void SetupCellTemplate()
    {
        base.SetupCellTemplate();
        Setup<CellGroup>();
    }
}