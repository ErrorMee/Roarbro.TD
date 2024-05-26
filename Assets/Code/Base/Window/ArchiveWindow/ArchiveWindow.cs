using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArchiveWindow : WindowBase
{
    public ArchiveList archiveList;

    [SerializeField] SDFBtn delBtn = default;
    [SerializeField] SDFBtn sureBtn = default;

    int selectIndex = -1;

    protected override void Awake()
    {
        base.Awake();
        AirModel.Add(transform);
        ClickListener.Add(delBtn.transform).onClick = OnClickDel;
        delBtn.gameObject.SetActive(false);
        ClickListener.Add(sureBtn.transform).onClick = OnClickSure;
        sureBtn.gameObject.SetActive(false);

        archiveList.OnSelected((index) =>
        {
            if (selectIndex == index)
            {
                CloseSelf();
                return;
            }
            ArchiveModel.Instance.Select(index);
            archiveList.SelectCell(index, false);
            selectIndex = index;

            delBtn.gameObject.SetActive(true);
            sureBtn.gameObject.SetActive(true);
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

    private void OnClickSure()
    {
        CloseSelf();
        Close(WindowEnum.Option);
    }

    private void OnConfirm()
    {
        ArchiveModel.Instance.Delete(ArchiveModel.Instance.Current);
        ArchiveModel.Instance.SaveArchives();
        archiveList.UpdateContents(ArchiveModel.Instance.Archives);
        selectIndex = -1;
        delBtn.gameObject.SetActive(false);
        sureBtn.gameObject.SetActive(false);
    }

    private void OnCancel() { }
}