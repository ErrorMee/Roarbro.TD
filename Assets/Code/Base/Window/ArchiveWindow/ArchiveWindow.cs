using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArchiveWindow : WindowBase
{
    public ArchiveList archiveList;

    protected override void Awake()
    {
        base.Awake();
        AirModel.Add(transform, AirCallback, AirEnum.Alpha);
    }

    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);

        archiveList.UpdateContents(ArchiveModel.Instance.Archives);
        archiveList.OnCellClicked((info) =>
        {
            ArchiveModel.Instance.Select(info);
            AirCallback();
        });
    }

    private void AirCallback()
    {
        CloseSelf();
        AirModel.Remove(transform);
    }
}