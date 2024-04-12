#if UNITY_EDITOR
//#define RIGOROUS_MODE // 严谨模式
#else 
//#define RIGOROUS_MODE
#endif

using System;
using System.Collections.Generic;
using UnityEngine;
//ConditionalAttribute("DEVELOPMENT_BUILD")

public class ObjectPool<T> : IDestroy where T : new()
{
	private readonly Func<T> factoryFunc;

	private readonly Queue<T> unUseItems;

#if RIGOROUS_MODE
    private readonly HashSet<T> usingItems;
#endif

    public ObjectPool()
	{
        factoryFunc = null;

        unUseItems = new Queue<T>();
#if RIGOROUS_MODE
        usingItems = new HashSet<T>();
#endif
	}

    public ObjectPool(Func<T> factoryFunc = null)
	{
		this.factoryFunc = factoryFunc;
		unUseItems = new Queue<T>();
#if RIGOROUS_MODE
        usingItems = new HashSet<T>();
#endif
	}

	public T Get()
	{
        unUseItems.TryDequeue(out T objectPoolItem);

        objectPoolItem ??= CreateItem();

#if RIGOROUS_MODE
        int allCount = usingItems.Count + unUseItems.Count;
        if (allCount > ushort.MaxValue)
		{
			Debug.LogWarning(string.Format("ObjectPool > {0} count:{1}", typeof(T).Name, allCount));
		}
		usingItems.Add(objectPoolItem);
#endif
        return objectPoolItem;
	}

	public void Cache(T instance)
	{
#if RIGOROUS_MODE
        if (usingItems.Remove(instance))
		{
			unUseItems.Enqueue(instance);
        }
		else
		{
			Debug.LogWarning("此对象不属于对象池: " + instance);
		}
#else
        unUseItems.Enqueue(instance);
#endif
    }

#if RIGOROUS_MODE
    public HashSet<T> GetUsingItems()
	{
		return usingItems;
	}
#endif

    public Queue<T> GetUnUseItems()
	{
		return unUseItems;
	}

	public void Clear()
	{
		unUseItems.Clear();
#if RIGOROUS_MODE
        usingItems.Clear();
#endif
	}

    public void Destroy()
    {
		Clear();
    }

    private T CreateItem()
	{
		T objectPoolItem = default;
		if (factoryFunc == null)
		{
			objectPoolItem = new T();
		}
		else
        {
			objectPoolItem = factoryFunc();
		}
        return objectPoolItem;
	}
}