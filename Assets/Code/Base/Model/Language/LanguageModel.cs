using System;
using System.Collections.Generic;
using UnityEngine;

public partial class LanguageModel : Singleton<LanguageModel>, IDestroy
{
    readonly Dictionary<int, LanguageConfig> languageDic = new();

    public int LanguageCrt
    {
        private set; get;
    }

    public void Init()
    {
        languageDic.Clear();
        foreach (var item in LanguageConfigs.All)
        {
            languageDic.Add(item.id, item);
        }

        InitSysLanguage();
        LanguageCrt = ArchiveModel.GetInt(ArchiveEnum.Language, LanguageCrt, false);
    }

    void InitSysLanguage()
    {
        LanguageCrt = 0;
        for (int i = 0; i < LanguageConfigs.Instance.types.Length; i++)
        {
            if (LanguageConfigs.Instance.types[i].type == Application.systemLanguage)
            {
                LanguageCrt = i;
                break;
            }
        }
    }

    public void Select(int language)
    {
        if (LanguageCrt != language)
        {
            LanguageCrt = language;
            ArchiveModel.SetInt(ArchiveEnum.Language, LanguageCrt, false);

            LanguageText[] languageTexts = WindowModel.Instance.GetComponentsInChildren<LanguageText>();
            foreach (LanguageText languageText in languageTexts)
            {
                languageText.RefreshText();
            }
        }
    }
}