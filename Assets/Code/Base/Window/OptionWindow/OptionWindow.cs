using System;
using TMPro;
using UnityEngine;

public class OptionWindow : WindowBase
{
    [SerializeField] SDFBtn languageBtn;

    [SerializeField] TextMeshProUGUI lang;

    [SerializeField] IntSwitch soundLv;
    [SerializeField] IntSwitch musicLv;

    override protected void Awake()
    {
        base.Awake();
        AirModel.Add(transform, AirCallback);

        AutoListener(EventEnum.LanguageChange, OnLanguageChange);
        ClickListener.Add(languageBtn.transform).onClick += OnClickLanguage;
        
        soundLv.switchCallBack = OnSoundChange;
        musicLv.switchCallBack = OnMusicChange;
    }

    private void AirCallback()
    {
        CloseSelf();
        AirModel.Remove(transform);
    }

    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);

        lang.text = LanguageConfigs.Instance.types[LanguageModel.Instance.LanguageCrt].tag;

        soundLv.Set(ArchiveModel.GetInt(ArchiveEnum.SoundVolume, AudioModel.DefSoundVolume, false), AudioModel.MaxVolume);
        musicLv.Set(ArchiveModel.GetInt(ArchiveEnum.MusicVolume, AudioModel.DefMusicVolume, false), AudioModel.MaxVolume);
    }

    void OnLanguageChange(object obj)
    {
        lang.text = LanguageConfigs.Instance.types[LanguageModel.Instance.LanguageCrt].tag;
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
}