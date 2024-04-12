using System;
using UnityEngine;

public partial class ArchiveModel : Singleton<ArchiveModel>, IDestroy
{
    const int MaxArchiveCount = 3;

    public ArchiveInfo[] Archives
    {
        get; private set;
    }

    public ArchiveInfo Current
    {
        get; private set;
    }

    public void Init()
    {
        Archives = GetObject<ArchiveInfo[]>(ArchiveEnum.Archives, false);
        if (Archives == null || Archives.Length != MaxArchiveCount)
        {
            Archives = new ArchiveInfo[MaxArchiveCount];
            for (int i = 0; i < Archives.Length; i++)
            {
                Archives[i] = new ArchiveInfo(i);
            }
        }

        for (int i = 0; i < Archives.Length; i++)
        {
            if (Archives[i].selected)
            {
                Select(Archives[i]);
                break;
            }
        }
        if (Current == null)
        {
            Select(Archives[0]);
        }
    }

    public void Select(int index)
    {
        Select(Archives[index]);
    }

    public void Select(ArchiveInfo archive)
    {
        for (int i = 0; i < Archives.Length; i++)
        {
            ArchiveInfo archiveItem = Archives[i];
            if (archiveItem == archive)
            {
                archiveItem.selected = true;
                Current = archiveItem;
            }
            else
            {
                archiveItem.selected = false;
            }
        }
    }

    public void Delete(ArchiveInfo archive)
    {
        foreach (ArchiveEnum key in Enum.GetValues(typeof(ArchiveEnum)))
        {
            PlayerPrefs.DeleteKey(archive.index + key.Str());
        }
    }

    public void SaveArchives()
    {
        SetObject(ArchiveEnum.Archives, Instance.Archives, false);
    }
}