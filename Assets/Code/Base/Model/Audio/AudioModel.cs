using System.Collections.Generic;
using UnityEngine;

public partial class AudioModel : SingletonBehaviour<AudioModel>
{
    readonly Dictionary<AudioGroupEnum, AudioGroupInfo> audioGroupInfos = new();

    public const int MaxVolume = 10;
    public const int DefSoundVolume = 8;
    public const int DefMusicVolume = 0;
    float soundVolume = 1;
    float musicVolume = 1;

    protected override void Awake()
    {
        base.Awake();
        audioGroupInfos.Clear();

        soundVolume = ArchiveModel.GetInt(ArchiveEnum.SoundVolume, AudioModel.DefSoundVolume, false) / (MaxVolume + 0f);
        musicVolume = ArchiveModel.GetInt(ArchiveEnum.MusicVolume, AudioModel.DefMusicVolume, false) / (MaxVolume + 0f);

        AddAudioGroupInfo(AudioGroupEnum.Music, 1, true, true);
        AddAudioGroupInfo(AudioGroupEnum.Sound, 16, false, false);
    }

    public void SetSoundVolumeLv(int lv)
    {
        ArchiveModel.SetInt(ArchiveEnum.SoundVolume, lv, false);
        soundVolume = lv / (MaxVolume + 0f);
    }

    public void SetMusicVolumeLv(int lv)
    {
        ArchiveModel.SetInt(ArchiveEnum.MusicVolume, lv, false);
        musicVolume = lv / (MaxVolume + 0f);
    }

    private void AddAudioGroupInfo(AudioGroupEnum audioEnum, int count, bool async, bool loop)
    {
        AudioSource[] audioSources = new AudioSource[count];
        for (int i = 0; i < count; i++)
        {
            string name = audioEnum.ToString() + i;
            GameObject obj = new(name);
            obj.transform.parent = transform;
            AudioSource audioSource = obj.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = loop;
            audioSources[i] = audioSource;
        }

        AudioGroupInfo audioGroupInfo = new()
        {
            audioSources = audioSources,
            async = async,
        };
        audioGroupInfos.Add(audioEnum, audioGroupInfo);
    }

    private void Play(AudioGroupEnum audioGroup, string audioName, float volume)
    {
        //Debug.Log("audioName " + audioName);
        AudioGroupInfo audioGroupInfo = audioGroupInfos[audioGroup];
        string address = Address.Audio(audioGroup, audioName);

        if (audioGroupInfo.async)
        {
            AddressModel.Instance.LoadAssetAsync<AudioClip>(address, (_result) =>
            {
                AudioClip audioClip = (AudioClip)_result;
                audioGroupInfo.Play(audioClip, volume);
            });
        }
        else
        {
            AudioClip audioClip = AddressModel.Instance.LoadAsset<AudioClip>(address);
            audioGroupInfo.Play(audioClip, volume);
        }
    }
}