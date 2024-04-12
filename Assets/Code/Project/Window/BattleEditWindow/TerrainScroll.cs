
public class TerrainScroll : ScrollList<TerrainEnum, TerrainCell>
{
    class CellGroup : DefaultCellGroup { }

    protected override void SetupCellTemplate()
    {
        base.SetupCellTemplate();
        Setup<CellGroup>();
    }
}