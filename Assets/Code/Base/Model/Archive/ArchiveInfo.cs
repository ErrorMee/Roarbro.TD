using System;

[Serializable]
public class ArchiveInfo
{
    public int index;
    public bool selected;
    public bool created;

    public ArchiveInfo(int index)
    {
        this.index = index;
    }
}