using System;
using System.Linq;
using UnityEngine;

public class LanguageWindow : WindowBase
{
    [SerializeField] LanguageScroll languageScroll = default;

    protected override void Awake()
    {
        base.Awake();

        AirModel.Add(transform, AirCallback);

        languageScroll.OnSelected((index) =>
        {
            languageScroll.SelectCell(index);
            LanguageModel.Instance.Select(index);
        });
    }

    private void AirCallback()
    {
        CloseSelf();
        SendEvent(EventEnum.LanguageSelect);
    }

    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);

        languageScroll.UpdateContents(LanguageConfigs.Instance.types.ToList());

        languageScroll.SelectCell(LanguageModel.Instance.LanguageCrt);
    }
}