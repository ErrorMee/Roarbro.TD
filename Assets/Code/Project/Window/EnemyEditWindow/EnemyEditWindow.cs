using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyEditWindow : WindowBase
{
    [SerializeField] EnemyScroll enemyScroll;

    /// <summary>
    /// ExecutionOrder: Awake>OnEnable>OnOpen>Start <see href="https://docs.unity3d.com/2020.3/Documentation/Manual/ExecutionOrder.html"/> 
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        enemyScroll.UpdateContents(EnemyConfigs.All);
        enemyScroll.UpdateSelection(BattleModel.Instance.battle.config.enemySelect);
        enemyScroll.OnCellClicked(OnScrollClicked);
    }

    /// <summary>
    /// OnOpen(object obj):Refreshable
    /// </summary>
    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);
        EnemyLayer.Edit = true;
    }

    private void OnScrollClicked(int index)
    {
        enemyScroll.UpdateSelection(index);
        BattleModel.Instance.battle.config.enemySelect = index;
        BattleConfigs.Instance.Save();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EnemyLayer.Edit = false;
    }
}