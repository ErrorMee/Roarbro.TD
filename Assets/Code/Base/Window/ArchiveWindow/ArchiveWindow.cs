using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArchiveWindow : WindowBase
{
    public ArchiveList archiveList;

    [SerializeField] SDFBtn delBtn = default;

    int selectIndex = -1;

    protected override void Awake()
    {
        base.Awake();
        AirModel.Add(transform);
        ClickListener.Add(delBtn.transform).onClick = OnClickDel;
        delBtn.gameObject.SetActive(false);

        archiveList.OnCellClicked((index) =>
        {
            if (selectIndex == index)
            {
                CloseSelf();
                return;
            }
            ArchiveModel.Instance.Select(index);
            archiveList.UpdateSelection(index, false);
            selectIndex = index;

            delBtn.gameObject.SetActive(true);
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
        selectIndex = -1;
        delBtn.gameObject.SetActive(false);
    }

    private void OnCancel() { }
}