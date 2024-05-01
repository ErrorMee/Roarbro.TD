using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ArchiveCell: ListCell<ArchiveInfo>
{
    [SerializeField] TextMeshProUGUI title = default;

    [SerializeField] protected SDFBtn btnDel = default;

    protected override void Awake()
    {
        base.Awake();
        ClickListener.Add(btnDel.transform).onClick = OnDel;
    }

    private void Start()
    {
        btnDel.transform.localScale = Vector3.zero;
    }

    public override void UpdateContent(ArchiveInfo info)
    {
        base.UpdateContent(info);
        if (info.enable)
        {
            title.text = LanguageModel.Get(10036) + " " + (info.index + 1);
            if (ArchiveModel.Instance.EnableArchiveCount() > 1)
            {
                btnDel.transform.DOScale(1, 0.2f).SetDelay(0.2f);
            }
            else
            {
                btnDel.transform.localScale = Vector3.zero;
            }
        }
        else
        {
            title.text = LanguageModel.Get(10046);
            btnDel.transform.localScale = Vector3.zero;
        }
    }

    private void OnDel()
    {
        WindowModel.Dialog(LanguageModel.Get(10035), LanguageModel.Get(10047), OnConfirm, OnCancel);
    }

    private void OnConfirm()
    {
        info.enable = false;
        ArchiveModel.Instance.SaveArchives();
        UpdateContent(info);
    }

    private void OnCancel() { }
}