using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// WaveFunctionCollapse
/// </summary>
public class WFC
{
    public WFCNode[,] nodes
    {
        set;
        get;
    }

    private int collapsedCount = 0;

    public bool CollapseError { private set; get; } = false;

    public void Init(int xCount, int zCount, List<WFCState> states)
    {
        if (nodes != null)
        {
            if (nodes.GetLength(0) != xCount || nodes.GetLength(1) != zCount)
            {
                nodes = new WFCNode[xCount, zCount];
            }
        }
        else
        {
            nodes = new WFCNode[xCount, zCount];
        }

        collapsedCount = 0;
        CollapseError = false;

        for (int z = 0; z < nodes.GetLength(1); z++)
        {
            for (int x = 0; x < nodes.GetLength(0); x++)
            {
                WFCNode node = new WFCNode(x, z);
                nodes[x, z] = node;

                if (states == null)
                {
                    continue;
                }
                HashSet<WFCState> statesClone = new();
                foreach (WFCState state in states)
                {
                    statesClone.Add(state);
                }
                node.states = statesClone;

                if (x > 0)
                {
                    node.SetRelation(Direction4.left, nodes[x - 1, z]);
                }
                if (z > 0)
                {
                    node.SetRelation(Direction4.back, nodes[x, z - 1]);
                }

                node.OnCollapseEvent = OnCollapseEvent;
            }
        }
    }

    public void CollapseNode(WFCNode node, WFCState state = null)
    {
        if (node.IsCollapsed)
        {
            return;
        }
        node.Collapse(state);
        Propagate(node);
    }

    private void Propagate(WFCNode _node)
    {
        Stack<WFCNode> stack = new();
        stack.Push(_node);

        while (stack.Count > 0)
        {
            WFCNode node = stack.Pop();

            for (int i = 0; i < node.relations.Length; i++)
            {
                WFCNode other = node.relations[i];

                if (other == null || other.IsCollapsed)
                {
                    continue;
                }

                if (node.RefreshStates(other))
                {
                    if (other.IsCollapsedError())
                    {
                        CollapseError = true;
                        return;
                    }
                    stack.Push(other);
                }
            }
        }
    }

    public void Collapse()
    {
        while (!IsCollapsed())
        {
            CollapseNode(GetMinEntropyNode());
        }
    }

    List<WFCNode> minEntropyNodes = new();
    public WFCNode GetMinEntropyNode()
    {
        minEntropyNodes.Clear();

        int referEntropy = int.MaxValue;

        for (int z = 0; z < nodes.GetLength(1); z++)
        {
            for (int x = 0; x < nodes.GetLength(0); x++)
            {
                WFCNode node = nodes[x, z];
                if (!node.IsCollapsed)
                {
                    if (node.states.Count == referEntropy)
                    {
                        minEntropyNodes.Add(node);
                    }
                    else if (node.states.Count < referEntropy)
                    {
                        referEntropy = node.states.Count;
                        minEntropyNodes.Clear();
                        minEntropyNodes.Add(node);
                    }
                }
            }
        }

        int index = Random.Range(0, minEntropyNodes.Count);
        return minEntropyNodes[index];
    }

    private void OnCollapseEvent(WFCNode node)
    {
        collapsedCount++;
    }

    private bool IsCollapsed()
    {
        if (CollapseError)
        {
            return true;
        }

        if (collapsedCount >= nodes.GetLength(0) * nodes.GetLength(1))
        {
            return true;
        }

        return false;
    }
}