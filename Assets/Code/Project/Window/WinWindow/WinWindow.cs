using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinWindow : WindowBase
{
    [SerializeField] SDFBtn confirmBtn;

    protected override void Awake()
    {
        base.Awake();
        AirModel.Add(transform);
        ClickListener.Add(confirmBtn.transform).onClick = OnClickConfirm;
    }

    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);
    }

    private void OnClickConfirm()
    {
        CloseSelf();
        
        AirModel.Remove(transform);
        WindowModel.Close(WindowEnum.Battle);
    }
}