using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : Singleton<GameObjectPool>, IDestroy
{
	/// <summary>
	/// 模板池列表
	/// </summary>
	private readonly Dictionary<GameObject, ObjectPool<GameObject>> templatePools = new();
#if UNITY_EDITOR
    /// <summary>
    /// 使用中的实例
    /// </summary>
    private readonly Dictionary<GameObject, ObjectPool<GameObject>> usingInstances = new();
#endif

	public void Init(GameObject template)
	{
		if (templatePools.ContainsKey(template))
		{
			Debug.LogError(template.name + " 对象池已存在");
			return;
		}
		ObjectPool<GameObject> pool = new(() =>
		{
			return GameObject.Instantiate(template, template.transform.parent);
		});
		templatePools[template] = pool;
	}

	public T Get<T>(GameObject template, Transform parent = null) where T: MonoBehaviour
	{
		GameObject gameObject = Get(template);
		if (parent != null)
		{
			gameObject.transform.parent = parent;
        }
		return gameObject.GetComponent<T>();
	}

	public GameObject Get(GameObject template)
	{
		if (!templatePools.ContainsKey(template))
		{
			Init(template);
		}

		ObjectPool<GameObject> templatePool = templatePools[template];

		GameObject instance = templatePool.Get();

		if (instance != null)
		{
			instance.SetActive(true);
#if UNITY_EDITOR
            if (!usingInstances.ContainsKey(instance))
			{
				usingInstances.Add(instance, templatePool);
			}
#endif
		}

		return instance;
	}

	static Vector3 far = new(short.MaxValue, 0, 0); 
	
	public void Cache(GameObject instance, bool activeFalse = true)
	{
		if (instance == null) { return; }

		if (activeFalse)
		{
			instance.SetActive(false);
		}
		else 
		{
			instance.transform.position = far;
        }

#if UNITY_EDITOR
        if (usingInstances.ContainsKey(instance))
		{
			usingInstances[instance].Cache(instance);
			usingInstances.Remove(instance);
		}
		else
		{
			Debug.LogWarning("此对象不属于对象池: " + instance.name);
		}
#endif
	}

	public void Clear(GameObject template)
	{
		if (templatePools.ContainsKey(template))
		{
			ObjectPool<GameObject> templatePool = templatePools[template];

#if RIGOROUS_MODE
            HashSet<GameObject> usingItems = templatePool.GetUsingItems();
			foreach(GameObject item in usingItems)
			{
				usingInstances.Remove(item);
                GameObject.Destroy(item);
            }
#endif

            Queue<GameObject> unUseItems = templatePool.GetUnUseItems();
			foreach (GameObject item in unUseItems) 
			{
				GameObject.Destroy(item);
			}
			templatePool.Clear();

			templatePools.Remove(template);
		}
	}
}