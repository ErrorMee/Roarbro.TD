using System;

public enum WFCSocketEnum : byte
{
    S1 = 1, S2 = 2,
}

[Serializable]
public class WFCSocket
{
    public WFCSocketEnum socket;

    public bool Match(WFCSocket targetSocket)
    {
        return socket == targetSocket.socket;
    }
}
