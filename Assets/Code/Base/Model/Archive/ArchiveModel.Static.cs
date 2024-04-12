using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
/// <summary>
/// Model.Static is used to simplify the use of model
/// </summary>
public partial class ArchiveModel
{
    public static void SetInt(ArchiveEnum key, int value, bool current = true)
    {
        if (current)
        {
            PlayerPrefs.SetInt(Instance.Current.index + key.Str(), value);
        }
        else
        {
            PlayerPrefs.SetInt(key.Str(), value);
        }
    }

    public static int GetInt(ArchiveEnum key, int defaultValue = 0, bool current = true)
    {
        if (current)
        {
            return PlayerPrefs.GetInt(Instance.Current.index + key.Str(), defaultValue);
        }
        else
        {
            return PlayerPrefs.GetInt(key.Str(), defaultValue);
        }
    }

    public static void SetString(ArchiveEnum key, string value, bool current = true)
    {
        if (current)
        {
            PlayerPrefs.SetString(Instance.Current.index + key.Str(), value);
        }
        else
        {
            PlayerPrefs.SetString(key.Str(), value);
        }
    }

    public static string GetString(ArchiveEnum key, string defaultValue = null, bool current = true)
    {
        if (current)
        {
            return PlayerPrefs.GetString(Instance.Current.index + key.Str(), defaultValue);
        }
        else
        {
            return PlayerPrefs.GetString(key.Str(), defaultValue);
        }
    }

    public static void SetObject(ArchiveEnum key, object data, bool current = true)
    {
        IFormatter formatter = new BinaryFormatter();
        string dataByte = string.Empty;
        using (MemoryStream stream = new())
        {
            formatter.Serialize(stream, data);
            byte[] byt = new byte[stream.Length];
            byt = stream.ToArray();
            dataByte = Convert.ToBase64String(byt);
            stream.Flush();
        }
        SetString(key, dataByte, current);
    }

    public static T GetObject<T>(ArchiveEnum key, bool current = true) where T : class
    {
        var dataByte = GetString(key, null, current);
        if (string.IsNullOrEmpty(dataByte)) { return default; }

        IFormatter formatter = new BinaryFormatter();
        byte[] byt = Convert.FromBase64String(dataByte);

        using Stream stream = new MemoryStream(byt, 0, byt.Length);
        object obj = formatter.Deserialize(stream);
        var classObj = obj as T;
        return classObj;
    }
}