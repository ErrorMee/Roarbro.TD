using System;
using System.Collections.Generic;

public enum WFCPrototypeEnum : byte
{
    P0 = 0,
    P1 = 1, P2, P3, P4, P5, P6,
}

[Serializable]
public class WFCPrototype : WFCState, IComparable
{
    public List<WFCState> GetStates()
    {
        List<WFCState> states = new List<WFCState>(4);
        states.Add(GetRotateState(0));
        if (left.socket == forward.socket
            && forward.socket == right.socket
            && right.socket == back.socket)
        {
        }
        else if (left.socket == right.socket
            && forward.socket == back.socket)
        {
            states.Add(GetRotateState(1));
        }
        else
        {
            states.Add(GetRotateState(1));
            states.Add(GetRotateState(2));
            states.Add(GetRotateState(3));
        }
        return states;
    }

    public WFCState GetRotateState(int index)
    {
        if (index == 0)
        {
            return this;
        }

        WFCState state = new WFCState(index);
        state.prototype = prototype;

        List<WFCSocket> sockets = new List<WFCSocket>();
        List<int> rotateIndexs = new List<int>();
        for (int i = 0; i < Sockets.Length; i++)
        {
            sockets.Add(Sockets[i]);

            int rotateIndex = i - index;
            if (rotateIndex < 0)
            {
                rotateIndex += Sockets.Length;
            }
            rotateIndexs.Add(rotateIndex);
        }

        for (int i = 0; i < Sockets.Length; i++)
        {
            state.Sockets[i] = sockets[rotateIndexs[i]];
        }
        state.left = state.Sockets[0];
        state.forward = state.Sockets[1];
        state.right = state.Sockets[2];
        state.back = state.Sockets[3];
        return state;
    }

    public int CompareTo(object obj)
    {
        return (int)(prototype) - (int)((WFCState)obj).prototype;
    }
}
