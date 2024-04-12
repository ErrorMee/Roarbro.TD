using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListView<TInfo, TCell> : MonoBehaviour
    where TInfo : class where TCell : ListCell<TInfo>
{
    public TCell cellPrefab;
    public readonly List<TCell> cells = new();

    protected Action<TInfo> clickCallBack;

    protected LayoutGroup layoutGroup;
    protected ContentSizeFitter contentSizeFitter;

    private int layoutFrame = 0;


    public void Awake()
    {
        layoutGroup = GetComponent<LayoutGroup>();
        contentSizeFitter = GetComponent<ContentSizeFitter>();
        cellPrefab.gameObject.SetActive(false);
    }

    public virtual void UpdateContents(IList<TInfo> infos)
    {
        layoutFrame = 10;
        if (contentSizeFitter != null)
        {
            contentSizeFitter.enabled = true;
        }
        if (layoutGroup != null)
        {
            layoutGroup.enabled = true;
        }
        int i = 0;
        for (; i < infos.Count; i++)
        {
            TInfo info = infos[i];
            TCell instance;
            if (i >= cells.Count)
            {
                instance = Instantiate(cellPrefab, transform);
                cells.Add(instance);
                instance.Init(OnClick, i);
            }
            else
            {
                instance = cells[i];
            }
            instance.UpdateContent(info);
            instance.gameObject.SetActive(true);
        }

        for (; i < cells.Count; i++)
        {
            TCell instance = cells[i];
            instance.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (layoutFrame > 0)
        {
            layoutFrame--;
            if (layoutFrame == 0 )
            {
                if (contentSizeFitter != null)
                {
                    contentSizeFitter.enabled = false;
                }
                if (layoutGroup != null)
                {
                    layoutGroup.enabled = false;
                }
            }
        }
    }

    private void OnClick(TInfo info)
    {
        clickCallBack?.Invoke(info);
    }

    public void OnCellClicked(Action<TInfo> callback)
    {
        clickCallBack = callback;
    }
}
