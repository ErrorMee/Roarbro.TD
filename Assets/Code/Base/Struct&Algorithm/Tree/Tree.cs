using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 树
/// </summary>
/// <typeparam name="T"></typeparam>
public class Tree<T>
{
    public T Data
    {
        private set;
        get;
    }

    public uint InstanceId
    {
        private set;
        get;
    }

    public Tree<T> Parent
    {
        set;
        get;
    }

    public List<Tree<T>> Children
    {
        private set;
        get;
    } = new List<Tree<T>>();

    public Tree(T data, uint instanceId = 0)
    {
        Data = data;
        InstanceId = instanceId;
    }

    public void AddChild(Tree<T> data)
    {
        if (data == this)
        {
            Debug.LogError("Tree不能是自己的Child");
            return;
        }

        if (Children.Contains(data))
        {
            Debug.LogError("Tree添加的Child重复");
            return;
        }

        Children.Add(data);
        data.Parent = this;
    }

    public void RemoveChild(Tree<T> data)
    {
        if (Children.Contains(data))
        {
            Children.Remove(data);
            data.Parent = null;
        }
    }

    public Tree<T> GetNode(T data, uint instanceId = 0)
    {
        if (Data.Equals(data) && InstanceId == instanceId)
        {
            return this;
        }
        else {

            for (int i = 0; i < Children.Count; i++)
            {
                Tree<T> child = Children[i];
                Tree<T> gchild = child.GetNode(data, instanceId);
                if (gchild != null)
                {
                    return gchild;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// 获取所有叶子节点
    /// </summary>
    /// <returns></returns>
    public List<Tree<T>> GetLeafs()
    {
        all.Clear();
        RecursiveGetAllLeafs(this, all);
        return all;
    }

    private void RecursiveGetAllLeafs(Tree<T> tree, List<Tree<T>> nodes)
    {
        if (tree.Children.Count == 0)
        {
            //nodes.Add(tree);
        }
        else
        {
            for (int i = 0; i < tree.Children.Count; i++)
            {
                RecursiveGetAllLeafs(tree.Children[i], nodes);
            }
        }
    }

    /// <summary>
    /// 获取某一层所有节点
    /// </summary>
    /// <param name="depth"></param>
    /// <returns></returns>
    public List<Tree<T>> GetAllAtDepth(uint depth)
    {
        all.Clear();
        RecursiveGetAllAtDepth(this, all, depth);
        return all;
    }

    private void RecursiveGetAllAtDepth(Tree<T> tree, List<Tree<T>> nodes, uint depth)
    {
        if (depth == 0)
        {
            return;
        }

        if (depth == 1)
        {
            nodes.Add(tree);
        }
        else
        {
            depth--;
            for (int i = 0; i < tree.Children.Count; i++)
            {
                RecursiveGetAllAtDepth(tree.Children[i], nodes, depth);
            }
        }
    }

    List<Tree<T>> all = new List<Tree<T>>();

    /// <summary>
    /// 获取所有节点
    /// </summary>
    /// <param name="maxDepth">最大层数，0为没有限制，1为自己，2是自己和直接Children 以此类推</param>
    /// <returns></returns>
    public List<Tree<T>> GetAll(uint maxDepth = 0)
    {
        all.Clear();
        RecursiveGetAll(this, all, maxDepth);
        return all;
    }

    private void RecursiveGetAll(Tree<T> tree, List<Tree<T>> nodes, uint maxDepth = 0)
    {
        nodes.Add(tree);
        if (maxDepth == 1)
        {
            return;
        }
        if (maxDepth > 1)
        {
            maxDepth--;
        }
        for (int i = 0; i < tree.Children.Count; i++)
        {
            RecursiveGetAll(tree.Children[i], nodes, maxDepth);
        }
    }
}
