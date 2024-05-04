using UnityEngine;
using DG.Tweening;

[DisallowMultipleComponent]
public class WindowBase : FrameMono
{
    [ReadOnlyProperty]
    public WindowConfig config;

    /// <summary>
    /// ExecutionOrder: Awake>OnEnable>OnOpen>Start <see href="https://docs.unity3d.com/2020.3/Documentation/Manual/ExecutionOrder.html"/> 
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// OnOpen(object obj):Refreshable
    /// </summary>
    public virtual void OnOpen(object obj)
    {
        switch (config.show)
        {
            case WindowShowEnum.Jelly:
                gameObject.transform.localScale = new(0.8f, 0.8f, 1);
                gameObject.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutBack);
                break;
            case WindowShowEnum.Alpha:
                CanvasModel.Instance.FadeIn(0.1f);
                break;
            case WindowShowEnum.Clamp:
                RectTransform rectTransform = gameObject.transform as RectTransform;
                Vector2 size = rectTransform.sizeDelta;
                Vector2 sizeScale = size + Vector2.one * 256;
                rectTransform.sizeDelta = sizeScale;
                rectTransform.DOSizeDelta(size, 0.2f).SetEase(Ease.OutCubic);
                break;
            default:
                break;
        }
        if (!isActiveAndEnabled)
        {
            gameObject.SetActive(true);
        }
        transform.SetAsLastSibling();

        foreach (var item in config.depends)
        {
            WindowModel.Open(item);
        }

        if (config.layer == WindowLayerEnum.Bot)
        {
            CameraModel.Instance.ShowBackGround(config.bg);
        }
    }

    protected void CloseSelf()
    {
        WindowModel.Close(config.id);
    }

    protected void Close(WindowEnum type)
    {
        WindowModel.Close(type);
    }

    protected void ReOpenSelf(object obj)
    {
        WindowModel.Open(config.id, obj);
    }

    protected void Open(WindowEnum type, object obj = null)
    {
        WindowModel.Open(type, obj);
    }

    protected virtual void OnDisable()
    {
        foreach (var item in config.depends)
        {
            WindowModel.Close(item);
        }
    }
}