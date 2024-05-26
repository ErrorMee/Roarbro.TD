using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyEditWindow : WindowBase
{
    [SerializeField] TextScrollFocus enemyScrollFoucus;

    [SerializeField] EnemyScroll enemyScroll;
    [SerializeField] IntSwitch levelSwitch;

    /// <summary>
    /// ExecutionOrder: Awake>OnEnable>OnOpen>Start <see href="https://docs.unity3d.com/2020.3/Documentation/Manual/ExecutionOrder.html"/> 
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        List<string> foucusDatas = new()
        {
            "0",
            "1",
            "2",
            "3",
            "4"
        };
        enemyScrollFoucus.UpdateContents(foucusDatas);
        enemyScrollFoucus.OnSelected(OnFoucusSelected);
        enemyScrollFoucus.SelectCell(2);


        enemyScroll.UpdateContents(EnemyConfigs.All);
        enemyScroll.UpdateSelection(EnemyModel.Instance.crtTemplate.enemyID);
        enemyScroll.OnCellClicked(OnScrollClicked);

        levelSwitch.prefix = LanguageModel.Get(10039) + ":";
        levelSwitch.Set(1, 99, 1);
        levelSwitch.switchCallBack = OnChangeLevel;
    }

    private void OnFoucusSelected(int index)
    {
        Debug.Log("OnFoucusChanged " + index);
    }

    /// <summary>
    /// OnOpen(object obj):Refreshable
    /// </summary>
    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);
        BattleModel.Instance.CreateLayer(typeof(EnemyLayer));
    }

    private void OnScrollClicked(int index)
    {
        enemyScroll.UpdateSelection(index);

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