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
        enemyScroll.UpdateSelection(EnemyModel.Instance.selectEnemy.enemyID);
        enemyScroll.OnCellClicked(OnScrollClicked);
    }

    /// <summary>
    /// OnOpen(object obj):Refreshable
    /// </summary>
    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);
        BattleModel.Instance.CreateLayer(typeof(EnemyLayer));
        EnemyLayer.fsm.ChangeState(EnemyLayerState.Edit);
    }

    private void OnScrollClicked(int index)
    {
        enemyScroll.UpdateSelection(index);

        EnemyModel.Instance.selectEnemy.enemyID = EnemyConfigs.Instance.GetConfigByIndex(index).id;
        //todo level
        EnemyModel.Instance.selectEnemy.level = 10;
        BattleConfigs.Instance.Save();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        BattleModel.Instance.DeleteLayer(typeof(EnemyLayer));
    }
}