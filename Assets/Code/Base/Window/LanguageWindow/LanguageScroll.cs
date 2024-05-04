using System;
using System.Collections.Generic;
using UnityEngine;

public class LanguageScroll : ScrollList<LanguageType, LanguageCell>
{
    class CellGroup : DefaultCellGroup { }

    protected override bool Scrollable
    {
        get { return false; }
    }

    protected override void SetupCellTemplate()
    {
        base.SetupCellTemplate();
        Setup<CellGroup>();
    }
}