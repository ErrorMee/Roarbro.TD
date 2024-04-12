using System;
using System.Collections.Generic;

/// <summary>
/// 行为树容器
/// </summary>
public class BTContainer : BTBase
{
    public List<BTBase> children;

    public BTContainer()
    {
        children = new List<BTBase>();
    }

    public void AddChild(BTBase child)
    {
        children.Add(child);
    }

    public void RecurveLeaf(Action<BTBase> callback)
    {
        for (int i = 0; i < children.Count; i++)
        {
            BTBase child = children[i];
            if (child is BTContainer)
            {
                (child as BTContainer).RecurveLeaf(callback);
            }
            else
            {
                callback.Invoke(children[i]);
            }
        }
    }

    public void RemoveChild(BTBase child)
    {
        int index = children.IndexOf(child);
        if (-1 != index)
        {
            children.Remove(child);
        }
    }

    public override void Clear()
    {
        base.Clear();
        children.Clear();
    }
}