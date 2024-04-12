using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 列表功能：定容、去重、按访问排序
/// </summary>
/// <typeparam name="T"></typeparam>
public class SmartList<T> : List<T>
{
    /// <summary>
    /// 定容：最大容量，默认0无限大，超出做队列弹出
    /// </summary>
    public int MaxCount { get; set; } = 0;

    /// <summary>
    /// 去重：是否支持重复元素
    /// </summary>
    public bool RepeatSupport { get; set; } = false;

    /// <summary>
    /// 在去重时是否按添加顺序排序。例如（1，2）add 1 =（2，1）
    /// </summary>
    public bool UseAddOrderWhenNoRepeat { get; set; } = true;

    public SmartList(int _MaxCount) : base(_MaxCount)
    {
        MaxCount = _MaxCount;
    }

    public new T Add(T item)
    {
        T removeItem = default;

        if (RepeatSupport)
        {
            base.Add(item);
        }
        else
        {
            int idx = base.IndexOf(item);
            if (idx >= 0)
            {
                if (UseAddOrderWhenNoRepeat)
                {
                    removeItem = base[idx];
                    base.RemoveAt(idx);
                    base.Add(item);
                }
            }
            else
            {
                base.Add(item);
            }
        }
        return removeItem;
    }

    /// <summary>
    /// 添加元素+返回重复或者超出元素
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public T PushPop(T item)
    {
        T popItem = this.Add(item);

        if (MaxCount > 0)
        {
            if (Count > MaxCount)
            {
                popItem = this[0];
                RemoveAt(0);
            }
        }

        return popItem;
    }
}
