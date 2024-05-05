using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScroll : ScrollList<EnemyConfig, EnemyCell>
{
    class CellGroup : DefaultCellGroup { }

    protected override void SetupCellTemplate()
    {
        base.SetupCellTemplate();
        Setup<CellGroup>();
    }

    //override protected bool Scrollable
    //{
    //    get { return false; }
    //}
}