using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArchiveCell: ScrollCell<ArchiveInfo>
{
    [SerializeField] TextMeshProUGUI title = default;

    [SerializeField] Graphic select = default;

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void UpdateContent(ArchiveInfo info)
    {
        base.UpdateContent(info);
        if (info.enable)
        {
            title.text = LanguageModel.Get(10036) + " " + (info.index + 1);
        }
        else
        {
            title.text = LanguageModel.Get(10046);
        }
        select.gameObject.SetActive(Index == Context.SelectedIndex);
    }
}