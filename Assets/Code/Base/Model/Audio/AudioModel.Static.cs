
public partial class AudioModel
{
    public static void PlaySound(string audioName, float volume = 1)
    {
        if (string.IsNullOrEmpty(audioName))
        {
            return;
        }
        Instance.Play(AudioGroupEnum.Sound, audioName, volume * Instance.soundVolume);
    }

    public static void PlaMusic(string audioName, float volume = 1)
    {
        if (string.IsNullOrEmpty(audioName))
        {
            return;
        }
        Instance.Play(AudioGroupEnum.Music, audioName, volume * Instance.musicVolume);
    }
}