using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleEditWindow : WindowBase
{
    [SerializeField] TerrainScroll terrainScroll;
    
    [SerializeField] SDFBtn quit;

    protected override void Awake()
    {
        base.Awake();

        ClickListener.Add(quit.transform).onClick += OnClickQiut;

        List<TerrainEnum> terrainSwitchStr = new List<TerrainEnum>();
        foreach (byte value in Enum.GetValues(typeof(TerrainEnum)))
        {
            terrainSwitchStr.Add((TerrainEnum)value);
        }
        terrainScroll.UpdateContents(terrainSwitchStr);
        terrainScroll.UpdateSelection((int)BattleModel.Instance.battle.config.terrainSelect);
        terrainScroll.OnCellClicked(OnSwitchClicked);
    }

    private void OnSwitchClicked(int index)
    {
        terrainScroll.UpdateSelection(index);
        BattleModel.Instance.battle.config.terrainSelect = (TerrainEnum)index;
    }

    void OnClickQiut()
    {
        CloseSelf();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        BattleModel.Instance.CloseBattle();
    }
}