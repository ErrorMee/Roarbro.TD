using System;

[Serializable]
public class ArchiveInfo
{
    public int index;
    public bool selected;
    public bool enable;

    public ArchiveInfo(int index)
    {
        this.index = index;
    }
}