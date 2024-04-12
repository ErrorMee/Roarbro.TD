using UnityEngine;
using System.Collections.Generic;
using System;

public class Singleton<T> where T : IDestroy, new()
{
    private static T _instance;
    private static readonly object syslock = new();

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (syslock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                        SingletonModel.AddSingleton(_instance);
                    }
                }
            }
            return _instance;
        }
    }

    public virtual void Destroy()
    {
        _instance = default;
    }
}

/// <summary>
/// 清理所有单例
/// </summary>
public class SingletonModel
{
    private static readonly List<IDestroy> m_Singletons = new();

    public static void AddSingleton(IDestroy singleton)
    {
        m_Singletons.Add(singleton);
    }

    /// <summary>
    /// 清理单个Singleton
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gcCollect"></param>
    public static void ClearOne<T>(bool gcCollect = false) where T : IDestroy, new()
    {
        Type checkType = typeof(T);
        for (int i = 0; i < m_Singletons.Count; i++)
        {
            IDestroy singleton = m_Singletons[i];

            if (checkType == singleton.GetType())
            {
                m_Singletons.RemoveAt(0);
                singleton.Destroy();
                if(gcCollect)
                {
                    GC.Collect();
                }
                break;
            }
        }
    }

    /// <summary>
    /// 清理所有Singleton
    /// </summary>
    public static void Clear()
    {
        while (m_Singletons.Count > 0)
        {
            IDestroy singleton = m_Singletons[^1];
            m_Singletons.RemoveAt(m_Singletons.Count - 1);
            singleton.Destroy();
        }
    }
}

public class SingletonBehaviour<T> : FrameMono where T : FrameMono
{
    static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    if (Application.isPlaying)
                    {
                        Debug.LogWarning("SingletonBehaviour: " + typeof(T).Name + " is null");
                        GameObject singletonObject = new() { name = typeof(T).Name };
                        instance = singletonObject.AddComponent<T>();
                    }
                    return null;
                }
            }
            return instance;
        }
    }
}