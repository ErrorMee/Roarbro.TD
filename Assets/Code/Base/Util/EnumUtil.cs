using System;
using System.Collections.Generic;

public static class EnumUtil
{
    static readonly Dictionary<AudioEnum, string> audioNames = new();
    static readonly Dictionary<ArchiveEnum, string> archiveNames = new();

    public static void Init()
    {
        foreach (short value in Enum.GetValues(typeof(AudioEnum)))
        {
            AudioEnum item = (AudioEnum)value;
            audioNames.Add(item, item.ToString());
        }

        audioNames[AudioEnum.none] = string.Empty;

        foreach (short value in Enum.GetValues(typeof(ArchiveEnum)))
        {
            ArchiveEnum item = (ArchiveEnum)value;
            archiveNames.Add(item, "." + item.ToString());
        }
    }

    public static string Str(this AudioEnum value)
    {
        return audioNames[value];
    }

    public static string Str(this ArchiveEnum value)
    {
        return archiveNames[value];
    }
}