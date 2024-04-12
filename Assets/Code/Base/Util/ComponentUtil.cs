using System;
using System.Collections.Generic;
using UnityEngine;

public static class ComponentUtil
{
    /// <summary>
    /// ��ȡ�����������
    /// </summary>
    /// <typeparam name="T">Ҫ��ȡ�����ӵ������</typeparam>
    /// <param name="gameObject">Ŀ�����</param>
    /// <returns>��ȡ�����ӵ������</returns>
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }

        return component;
    }
}