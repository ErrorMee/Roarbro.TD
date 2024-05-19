using System;
using System.Collections.Generic;
using UnityEngine;

public partial class GridModel : Singleton<GridModel>, IDestroy
{
    private readonly Dictionary<Type, Dictionary<Vector2Int, HashSet<MonoBehaviour>>> gridDic = new();

    public Dictionary<Vector2Int, HashSet<MonoBehaviour>> GetGrid<T>() where T : MonoBehaviour
    {
        Type type = typeof(T);
        if (gridDic.ContainsKey(type) == false)
        {
            gridDic.Add(type, new Dictionary<Vector2Int, HashSet<MonoBehaviour>>());
        }
        return gridDic[type];
    }

    public void DeletetGrid<T>() where T : MonoBehaviour
    {
        Type type = typeof(T);
        gridDic.Remove(type);
    }

    public HashSet<MonoBehaviour> GetItems<T>(Vector2Int coord) where T : MonoBehaviour
    {
        Dictionary<Vector2Int, HashSet<MonoBehaviour>> grid = GetGrid<T>();
        grid.TryGetValue(coord, out HashSet<MonoBehaviour> items);
        return items;
    }

    public void UpdatePos<T>(T item, Vector3 newLocalPos) where T : MonoBehaviour
    {
        Dictionary<Vector2Int, HashSet<MonoBehaviour>> grid = GetGrid<T>();

        Vector2Int coordPre = new Vector2Int(
            Mathf.RoundToInt(item.transform.localPosition.x), 
            Mathf.RoundToInt((item.transform.localPosition.z)));
        item.transform.localPosition = newLocalPos;

        Vector2Int coordNew = new Vector2Int(
           Mathf.RoundToInt(newLocalPos.x),
           Mathf.RoundToInt((newLocalPos.z)));

        grid.TryGetValue(coordNew, out HashSet<MonoBehaviour> itemsNew);
        if (itemsNew == null)
        {
            itemsNew = new HashSet<MonoBehaviour>();
            grid.Add(coordNew, itemsNew);
        }
        itemsNew.Add(item);

        if (coordNew != coordPre)
        {
            grid.TryGetValue(coordPre, out HashSet<MonoBehaviour> itemsPre);
            itemsPre?.Remove(item);
        }
    }

    public void RemoveItem<T>(T item) where T : MonoBehaviour
    {
        Dictionary<Vector2Int, HashSet<MonoBehaviour>> grid = GetGrid<T>();
        Vector2Int coord = new Vector2Int(
            Mathf.RoundToInt(item.transform.localPosition.x),
            Mathf.RoundToInt((item.transform.localPosition.z)));

        if (grid.TryGetValue(coord, out HashSet<MonoBehaviour> items))
        {
            items.Remove(item);
        }
    }
}