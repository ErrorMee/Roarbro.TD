using System;
using System.Linq;
using UnityEngine;

public class LanguageWindow : WindowBase
{
    [SerializeField] LanguageScroll languageScroll = default;

    protected override void Awake()
    {
        base.Awake();

        AirModel.Add(transform, AirCallback, AirEnum.Alpha);

        languageScroll.OnCellClicked((index) =>
        {
            languageScroll.UpdateSelection(index);
            LanguageModel.Instance.Set(index);
        });
    }

    private void AirCallback()
    {
        CloseSelf();
        SendEvent(EventEnum.LanguageChange);
        AirModel.Remove(transform);
    }

    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);

        languageScroll.UpdateContents(LanguageConfigs.Instance.types.ToList());

        languageScroll.UpdateSelection(LanguageModel.Instance.LanguageCrt);
    }
}