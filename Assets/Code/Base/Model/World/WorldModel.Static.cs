using System;
using UnityEngine;

public partial class WorldModel : SingletonBehaviour<WorldModel>
{
    public static GameObject AddLayer(Type type)
    {
        if (Instance.layers.ContainsKey(type) == false)
        {
            GameObject worldLayer = new(type.Name);
            worldLayer.transform.SetParent(Instance.transform, false);
            worldLayer.AddComponent(type);
            Instance.layers.Add(type, worldLayer);
            return worldLayer;
        }
        else
        {
            return Instance.layers[type];
        }
    }

    public static void RemoveLayer(Type type)
    {
        if (Instance.layers.ContainsKey(type))
        {
            Destroy(Instance.layers[type]);
            Instance.layers.Remove(type);
        }
    }

    public static void ShowLayer(Type type, bool show)
    {
        if (Instance.layers.ContainsKey(type))
        {
            Instance.layers[type].SetActive(show);
        }
    }
}