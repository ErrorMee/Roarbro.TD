using System;
using System.Collections.Generic;
using UnityEngine;

public class WFCNode
{
    public HashSet<WFCState> states;

    public WFCNode[] relations = new WFCNode[4];

    public WFCState collapse;
    public bool IsCollapsed { private set; get; } = false;

    public Action<WFCNode> OnCollapseEvent;

    public Vector2Int pos;

    public WFCNode(int x, int y)
    {
        pos.x = x; pos.y = y;
    }

    public void SetRelation(Direction4 direction, WFCNode node)
    {
        if (relations[(int)direction] != null)
        { return; }

        if (node != null)
        {
            relations[(int)direction] = node;
            node.relations[(int)direction.Oppo()] = this;
        }
    }

    public bool IsCollapsedError()
    {
        return IsCollapsed && (collapse == null);
    }

    public void Collapse(WFCState state = null)
    {
        if (state != null)
        {
            OnCollapse(state);
        }
        else
        {
            if (states.Count == 0)
            {
                OnCollapse(null);
            }
            else
            {
                RandomCollapse();
            }
        }
    }

    private void RandomCollapse()
    {
        List<WFCState> maxWeightGroup = new List<WFCState>();
        WFCState represent = null;
        foreach (WFCState state in states)
        {
            int weight = state.weight;

            if (represent == null || represent.weight == weight)
            {
                maxWeightGroup.Add(state);
                represent = state;
            }
            else if (represent.weight < weight)
            {
                maxWeightGroup.Clear();
                maxWeightGroup.Add(state);
                represent = state;
            }
        }

        int index = UnityEngine.Random.Range(0, maxWeightGroup.Count);
        OnCollapse(maxWeightGroup[index]);
    }

    private void OnCollapse(WFCState state)
    {
        if (state == null)
        {
            Debug.LogWarning("OnCollapse state == null");
        }
        IsCollapsed = true;
        collapse = state;
        states.Clear();
        OnCollapseEvent?.Invoke(this);
    }

    public bool RefreshStates(WFCNode other)
    {
        HashSet<WFCState> otherEntropys = GetOtherPossibleStates(other);
        if (otherEntropys.Count != other.states.Count)
        {
            other.UpdateStates(otherEntropys);
            return true;
        }
        return false;
    }

    private void UpdateStates(HashSet<WFCState> _Entropys)
    {
        states = _Entropys;
        if (states.Count == 0)
        {
            OnCollapse(null);
        }
    }

    public HashSet<WFCState> GetOtherPossibleStates(WFCNode other)
    {
        HashSet<WFCState> possibleStates = new HashSet<WFCState>();

        if (collapse != null)
        {
            GetMatchStates(possibleStates, collapse, other);
        }
        else
        {
            foreach (WFCState state in states)
            {
                GetMatchStates(possibleStates, state, other);
            }
        }
        return possibleStates;
    }

    private void GetMatchStates(HashSet<WFCState> matchStates, WFCState entropy, WFCNode other)
    {
        int socketIndex = Array.IndexOf(relations, other);
        int oppoSocketIndex = entropy.OppoSocketIndex(socketIndex);

        WFCSocket socket = entropy.Sockets[socketIndex];
        foreach (WFCState otherEntropy in other.states)
        {
            WFCSocket otherSocket = otherEntropy.Sockets[oppoSocketIndex];

            if (socket.Match(otherSocket))
            {
                matchStates.Add(otherEntropy);
            }
        }
    }
}