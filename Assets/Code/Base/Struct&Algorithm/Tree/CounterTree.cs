using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 计数树
/// </summary>
/// <typeparam name="T"></typeparam>
public class CounterTree<T> : Tree<T>
{
    public int Counter
    {
        private set;
        get;
    }

    public CounterTree(T data, uint instanceId = 0) :base(data, instanceId)
    {
        
    }

    public void RemoveChild(CounterTree<T> data)
    {
        if (Children.Contains(data))
        {
            Children.Remove(data);
            Reduce(Counter);
            data.Parent = null;
        }
    }

    /// <summary>
    /// 增加
    /// </summary>
    /// <param name="value"></param>
    public void InCrease(int value = 1)
    {
        Counter += value;
        if (Parent != null)
        {
            (Parent as CounterTree<T>).InCrease(value);
        }
    }

    public void ClearCouter()
    {
        if (Parent != null)
        {
            (Parent as CounterTree<T>).Reduce(Counter);
        }
        Counter = 0;
    }

    /// <summary>
    /// 减少
    /// </summary>
    /// <param name="value"></param>
    public void Reduce(int value = 1)
    {
        Counter -= value;
        if (Parent != null)
        {
            (Parent as CounterTree<T>).Reduce(value);
        }
    }
}
