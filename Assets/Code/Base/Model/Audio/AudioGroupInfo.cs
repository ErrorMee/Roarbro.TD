using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGroupInfo
{
    public string name;

    public AudioSource[] audioSources;

    public int index;

    public bool async = true;

    public bool loop = false;

    public void Play(AudioClip audioClip, float volume)
    {
        if (index == audioSources.Length - 1)
        {
            index = 0;
        }
        AudioSource audioSource = audioSources[index];
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        index++;
    }
}
