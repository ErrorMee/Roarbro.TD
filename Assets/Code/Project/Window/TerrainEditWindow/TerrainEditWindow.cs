using System;
using System.Collections.Generic;
using UnityEngine;

public class TerrainEditWindow : WindowBase
{
    [SerializeField] TerrainScroll terrainScroll;

    protected override void Awake()
    {
        base.Awake();

        List<TerrainEnum> terrains = new List<TerrainEnum>();
        foreach (byte value in Enum.GetValues(typeof(TerrainEnum)))
        {
            terrains.Add((TerrainEnum)value);
        }
        terrainScroll.UpdateContents(terrains);
        terrainScroll.UpdateSelection((int)BattleModel.Instance.battle.config.terrainSelect);
        terrainScroll.OnCellClicked(OnScrollClicked);
    }

    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);
        EditLayer.Start = true;
    }

    private void OnScrollClicked(int index)
    {
        terrainScroll.UpdateSelection(index);
        BattleModel.Instance.battle.config.terrainSelect = (TerrainEnum)index;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EditLayer.Start = false;
    }
}