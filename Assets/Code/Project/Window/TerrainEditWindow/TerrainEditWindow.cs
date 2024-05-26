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
        terrainScroll.SelectCell((int)BattleModel.Instance.battle.config.terrainSelect);
        terrainScroll.OnSelected(OnScrollClicked);
    }

    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);
        TerrainLayer.Edit = true;
    }

    private void OnScrollClicked(int index)
    {
        terrainScroll.SelectCell(index);
        BattleModel.Instance.battle.config.terrainSelect = (TerrainEnum)index;
        BattleConfigs.Instance.Save();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        TerrainLayer.Edit = false;
    }
}