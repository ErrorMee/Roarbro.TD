using System;
using TMPro;
using UnityEngine;

public class OptionWindow : WindowBase
{
    [SerializeField] SDFBtn languageBtn;
    [SerializeField] TextMeshProUGUI lang;

    [SerializeField] IntSwitch soundLv;
    [SerializeField] IntSwitch musicLv;

    [SerializeField] SDFBtn archiveBtn;
    [SerializeField] TextMeshProUGUI archive;

    override protected void Awake()
    {
        base.Awake();
        AirModel.Add(transform, AirCallback);

        AutoListener(EventEnum.LanguageSelect, OnLanguageSelect);
        AutoListener(EventEnum.ArchiveSelect, OnLanguageSelect);

        ClickListener.Add(languageBtn.transform).onClick += OnClickLanguage;
        
        soundLv.switchCallBack = OnSoundChange;
        musicLv.switchCallBack = OnMusicChange;

        ClickListener.Add(archiveBtn.transform).onClick += OnClickArchive;
    }

    private void AirCallback()
    {
        CloseSelf();
    }

    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);

        OnLanguageSelect();

        soundLv.Set(ArchiveModel.GetInt(ArchiveEnum.SoundVolume, AudioModel.DefSoundVolume, false), AudioModel.MaxVolume, 0);
        musicLv.Set(ArchiveModel.GetInt(ArchiveEnum.MusicVolume, AudioModel.DefMusicVolume, false), AudioModel.MaxVolume, 0);
    }

    void OnLanguageSelect(object obj = null)
    {
        lang.text = LanguageConfigs.Instance.types[LanguageModel.Instance.LanguageCrt].tag;
        archive.text = LanguageModel.Get(10036) + " " + (ArchiveModel.Instance.Current.index + 1);
    }

    void OnClickLanguage()
    {
        WindowModel.Open(WindowEnum.Language);
    }

    void OnSoundChange(int index)
    {
        AudioModel.Instance.SetSoundVolumeLv(index);
    }

    void OnMusicChange(int index)
    {
        AudioModel.Instance.SetMusicVolumeLv(index);
    }

    void OnClickArchive()
    {
        WindowModel.Open(WindowEnum.Archive);
    }
}