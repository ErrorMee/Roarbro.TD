using UnityEngine;

public class TextScroll : ScrollList<string, TextCell> 
{
    class CellGroup : DefaultCellGroup { }

    protected override void SetupCellTemplate()
    {
        base.SetupCellTemplate();
        Setup<CellGroup>();
    }
}