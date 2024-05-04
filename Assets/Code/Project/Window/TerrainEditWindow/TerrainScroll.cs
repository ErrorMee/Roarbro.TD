
public class TerrainScroll : ScrollList<TerrainEnum, TerrainCell>
{
    class CellGroup : DefaultCellGroup { }

    protected override void SetupCellTemplate()
    {
        base.SetupCellTemplate();
        Setup<CellGroup>();
    }

    override protected bool Scrollable
    {
        get { return false; }
    }
}