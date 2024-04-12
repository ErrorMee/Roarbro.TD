using System;
using UnityEngine;
using UnityEngine.UI;

public class ListCell<TInfo> : MonoBehaviour
{
    [SerializeField] protected SDFBtn btn = default;

    [HideInInspector]
    public TInfo info;

    protected Action<TInfo> clickCallBack;

    public int Index
    {
        get;private set;
    }

    protected virtual void Awake()
    {
        if (btn != null)
        {
            ClickListener.Add(btn.transform).onClick = OnClick;
        }
    }

    protected virtual void OnClick()
    {
        clickCallBack.Invoke(info);
    }

    public virtual void Init(Action<TInfo> callback, int index)
    {
        clickCallBack = callback;
        Index = index;
    }

    public virtual void UpdateContent(TInfo info)
    {
        this.info = info;
    }
}