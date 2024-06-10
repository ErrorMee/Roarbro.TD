using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseWindow : WindowBase
{
    [SerializeField] SDFBtn quitBtn;
    [SerializeField] SDFBtn backBtn;

    /// <summary>
    /// ExecutionOrder: Awake>OnEnable>OnOpen>Start <see href="https://docs.unity3d.com/2020.3/Documentation/Manual/ExecutionOrder.html"/> 
    /// </summary>
    protected override void Awake()
    {
        AirModel.Add(transform);
        ClickListener.Add(quitBtn.transform).onClick = OnClickQuit;
        ClickListener.Add(backBtn.transform).onClick = OnClickBack;
    }

    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);
    }

    private void OnClickQuit()
    {
        CloseSelf();

        WindowModel.Close(WindowEnum.Battle);
    }

    private void OnClickBack()
    {
        CloseSelf();
    }
}