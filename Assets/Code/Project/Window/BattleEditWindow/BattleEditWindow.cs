using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleEditWindow : WindowBase
{
    [SerializeField] SDFBtn quit;

    protected override void Awake()
    {
        base.Awake();

        ClickListener.Add(quit.transform).onClick += OnClickQiut;

        Open(WindowEnum.TerrainEdit);
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