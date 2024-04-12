using System;
using UnityEngine;

[Serializable]
public class WFCState : WFCStateBase
{
    public WFCSocket left = null;
    public WFCSocket forward = null;
    public WFCSocket right = null;
    public WFCSocket back = null;

    private WFCSocket[] _Sockets;
    public WFCSocket[] Sockets
    {
        get
        {
            if (_Sockets == null || _Sockets.Length < 1)
            {
                _Sockets = new WFCSocket[4] { left, forward, right, back };
            }
            return _Sockets;
        }
    }

    [Range(0, 8)]
    public int weight = 0;

    public WFCState(int rotate = 0)
    {
        this.rotate = rotate;
    }

    public int OppoSocketIndex(int index)
    {
        return index < 2 ? (index + 2) : (index - 2);
    }

    public bool HasSocket(WFCSocketEnum socketType)
    {
        foreach (WFCSocket socket in Sockets)
        {
            if (socket.socket == socketType)
            {
                return true;
            }
        }
        return false;
    }

    public int CountSocket(WFCSocketEnum socketType)
    {
        int count = 0;
        foreach (WFCSocket socket in Sockets)
        {
            if (socket.socket == socketType)
            {
                count ++;
            }
        }
        return count;
    }

}
