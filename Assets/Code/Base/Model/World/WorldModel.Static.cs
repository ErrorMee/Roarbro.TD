using System;
using System.Collections.Generic;
using UnityEngine;

public partial class WorldModel : SingletonBehaviour<WorldModel>
{
    public static void CreateLayer(Type type, int depth)
    {
        if (Instance.layers.ContainsKey(type) == false)
        {
            GameObject battleLayer = new(type.Name);
            battleLayer.transform.SetParent(Instance.transform, false);
            battleLayer.AddComponent(type);
            battleLayer.transform.position = new Vector3(0, depth * 0.01f, 0);
            Instance.layers.Add(type, battleLayer.transform);
        }
    }

    public static void DeleteLayer(Type type)
    {
        if (Instance.layers.ContainsKey(type))
        {
            Destroy(Instance.layers[type].gameObject);
            Instance.layers.Remove(type);
        }
    }

    public static void ShowLayer(Type type, bool show)
    {
        if (Instance.layers.ContainsKey(type))
        {
            Instance.layers[type].gameObject.SetActive(show);
        }
    }
}