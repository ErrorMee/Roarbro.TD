using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Configs<S, C> : ConfigsGenerics<C> where S : ConfigsGenerics<C> where C : Config
{
    static S _instance;
    public static S Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = ConfigModel.Instance.GetConfigs<S, C>();
            }
            return _instance;
        }
        set { _instance = value; }
    }

    public void Save()
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssetIfDirty(this);
#endif
    }

    static Dictionary<string, S> _instances;
    public static S InstanceWidthKey(string key)
    {
        _instances ??= new Dictionary<string, S>();
        if (!_instances.ContainsKey(key))
        {
            _instances.Add(key, ConfigModel.Instance.GetConfigs<S, C>(key));
        }
        return _instances[key];
    }

    public static C[] All
    {
        get
        {
            return Instance.all;
        } 
    }

    public static C ConfigByID(int id)
    {
        for (int i = 0; i < Instance.all.Length; i++)
        {
            C config = Instance.all[i];
            if (config.id == id)
            {
                return config;
            }
        }
        return null;
    }

    public C GetConfigByID(int id)
    {
        for (int i = 0; i < all.Length; i++)
        {
            C config = all[i];
            if (config.id == id)
            {
                return config;
            }
        }
        return null;
    }

    public static C ConfigByIndex(int index)
    {
        return Instance.all[index];
    }

    public C GetConfigByIndex(int index)
    {
        return all[index];
    }
}

public class ConfigsBase : ScriptableObject
{
    public bool showDel = false;

    public bool showAdd = false;

    public bool showUse = false;

    public bool showIdx = false;

    public bool showId = false;

    public bool showName = false;

    public int pageRow = 10;
    public int pageColumn = 1;
    public int pageCrt = 1;

    public string key = string.Empty;

    public int GetPageCapacity()
    {
        return pageRow * pageColumn;
    }
}

public class ConfigsGenerics<C> : ConfigsBase
{
    public C[] all;
}