using System;
using System.Collections.Generic;
using UnityEngine;

public class PieceUnit : BattleUnit
{
    [ReadOnlyProperty]
    public int posx;
    [ReadOnlyProperty]
    public int posy;

    protected override void OnEnable() { }
    protected override void OnDisable() { }

    public void UpdateShow()
    {
        int randomID = UnityEngine.Random.Range(0, 6);
        meshRenderer.SetMPBInt(MatPropUtil.IndexKey, randomID, false);
        int randomQuality = UnityEngine.Random.Range(0, QualityConfigs.All.Length);
        Color color = QualityConfigs.GetColor(randomQuality);
        meshRenderer.SetMPBColor(MatPropUtil.BaseColorKey, color);
    }
}