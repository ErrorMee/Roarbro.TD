using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyEditWindow : WindowBase
{
    [SerializeField] EnemySFocus enemyFoucus;

    [SerializeField] IntSwitch levelSwitch;

    /// <summary>
    /// ExecutionOrder: Awake>OnEnable>OnOpen>Start <see href="https://docs.unity3d.com/2020.3/Documentation/Manual/ExecutionOrder.html"/> 
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        enemyFoucus.UpdateContents(EnemyConfigs.All);
        enemyFoucus.OnSelected(OnEnemySelected);
        enemyFoucus.SelectCell(EnemyModel.Instance.crtTemplate.enemyID);

        levelSwitch.prefix = LanguageModel.Get(10039) + ":";
        levelSwitch.Set(1, 99, 1);
        levelSwitch.switchCallBack = OnChangeLevel;
    }

    /// <summary>
    /// OnOpen(object obj):Refreshable
    /// </summary>
    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);
        BattleModel.Instance.CreateLayer(typeof(EnemyLayer));
    }

    private void OnEnemySelected(int index)
    {
        EnemyModel.Instance.SelectTemplate(index);

        levelSwitch.Set(EnemyModel.Instance.crtTemplate.level);
    }

    protected override void OnDestroy()
    {
        if (ProjectConfigs.isQuiting == false)
        {
            base.OnDestroy();
            BattleModel.Instance.DeleteLayer(typeof(EnemyLayer));
        }
    }

    private void OnChangeLevel(int value)
    {
        EnemyModel.Instance.crtTemplate.level = value;
    }
}