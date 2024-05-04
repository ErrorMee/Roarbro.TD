using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArchiveWindow : WindowBase
{
    public ArchiveList archiveList;

    [SerializeField] protected SDFBtn delBtn = default;

    int selectIndex = -1;

    protected override void Awake()
    {
        base.Awake();
        AirModel.Add(transform, AirCallback, AirEnum.Alpha);
        ClickListener.Add(delBtn.transform).onClick = OnClickDel;

        archiveList.OnCellClicked((index) =>
        {
            if (selectIndex == index)
            {
                AirCallback();
                ArchiveModel.Instance.Select(index);
                return;
            }
            archiveList.UpdateSelection(index, false);
            selectIndex = index;
        });
    }

    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);

        archiveList.UpdateContents(ArchiveModel.Instance.Archives);
    }

    private void OnClickDel()
    {
        WindowModel.Dialog(LanguageModel.Get(10035), LanguageModel.Get(10047), OnConfirm, OnCancel);
    }

    private void OnConfirm()
    {
        ArchiveModel.Instance.Delete(ArchiveModel.Instance.Current);
        ArchiveModel.Instance.SaveArchives();
        archiveList.UpdateContents(ArchiveModel.Instance.Archives);
    }

    private void OnCancel() { }

    private void AirCallback()
    {
        CloseSelf();
        AirModel.Remove(transform);
    }
}