
using System;
using UnityEngine;

public static class GameObjectUtil
{
    /// <summary>
    /// 获取 GameObject 是否在场景中。
    /// </summary>
    /// <param name="gameObject">目标对象。</param>
    /// <returns>GameObject 是否在场景中。</returns>
    /// <remarks>若返回 true，表明此 GameObject 是一个场景中的实例对象；若返回 false，表明此 GameObject 是一个 Prefab。</remarks>
    public static bool InScene(this GameObject gameObject)
    {
        return gameObject.scene.name != null;
    }

    /// <summary>
    /// 递归设置游戏对象的层次。
    /// </summary>
    /// <param name="gameObject"><see cref="UnityEngine.GameObject" /> 对象。</param>
    /// <param name="layer">目标层次的编号。</param>
    public static void SetLayerRecursively(this GameObject gameObject, int layer)
    {
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < transforms.Length; i++)
        {
            transforms[i].gameObject.layer = layer;
        }
    }

    /// <summary>
    /// 创建子物体。
    /// </summary>
    public static T CreateChild<T>(GameObject itemPref, GameObject parent = null)
    {
        var obj = GameObject.Instantiate(itemPref);
        if (parent != null)
        {
            obj.transform.SetParent(parent.transform);
        }
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
		obj.transform.localRotation = Quaternion.Euler(Vector3.zero);	

		return obj.GetComponent<T>();
    }

	/// <summary>
	/// 获取GameObject的HierachyName
	/// </summary>
	/// <param name="gameObject"></param>
	/// <returns></returns>
	public static string GetHierachyName(GameObject gameObject, bool withoutRoot = false)
	{
		if (gameObject == null)
		{
			return null;
		}
		string hierachyName = gameObject.name;
		GameObject crt = gameObject;
		while (crt.transform.parent != null && (withoutRoot ? crt.transform.parent.parent != null : true))
		{
			hierachyName = crt.transform.parent.name + "/" + hierachyName;
			crt = crt.transform.parent.gameObject;
		}
		return hierachyName;
	}
}
