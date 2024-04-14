using System;
using System.Collections.Generic;
using UnityEngine;

public class PieceUnit : BattleUnit
{
    [ReadOnlyProperty]
    public PieceInfo info;

    protected override void OnEnable() { }
    protected override void OnDisable() { }

    public void UpdateShow()
    {
        transform.localPosition = new Vector3(info.GetViewX(), 0, info.GetViewZ());

        meshRenderer.SetMPBInt(MatPropUtil.IndexKey, info.type, false);
        Color color = QualityConfigs.GetColor(info.level);
        meshRenderer.SetMPBColor(MatPropUtil.BaseColorKey, color);
    }
}