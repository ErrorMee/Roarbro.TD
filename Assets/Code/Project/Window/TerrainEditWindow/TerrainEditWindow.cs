using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainEditWindow : WindowBase
{
    [SerializeField] TerrainScroll terrainScroll;

    protected override void Awake()
    {
        base.Awake();

        List<TerrainEnum> terrainSwitchStr = new List<TerrainEnum>();
        foreach (byte value in Enum.GetValues(typeof(TerrainEnum)))
        {
            terrainSwitchStr.Add((TerrainEnum)value);
        }
        terrainScroll.UpdateContents(terrainSwitchStr);
        terrainScroll.UpdateSelection((int)BattleModel.Instance.battle.config.terrainSelect);
        terrainScroll.OnCellClicked(OnSwitchClicked);
    }

    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);
    }

    private void OnSwitchClicked(int index)
    {
        terrainScroll.UpdateSelection(index);
        BattleModel.Instance.battle.config.terrainSelect = (TerrainEnum)index;
    }
}