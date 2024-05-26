using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasRenderer))]
[AddComponentMenu("SDFUI/SDFBtn", 2)]
public class SDFBtn : Button
{
    protected Vector3 initScale;

    protected override void Awake()
    {
        base.Awake();
        initScale = transform.localScale;
        if (Application.isPlaying)
        {
            ClickListener.Add(gameObject).onDown = OnDown;
            ClickListener.Add(gameObject).onUp = OnUp;
            ClickListener.Add(gameObject).onEnter = OnEnter;
            ClickListener.Add(gameObject).onExit = OnExit;
        }
    }

    protected virtual void OnDown()
    {
        TweenScale(initScale * 1.1f);
    }

    protected virtual void OnUp()
    {
        TweenScale(initScale);
    }

    protected virtual void OnEnter()
    {
        TweenScale(initScale * 1.1f);
    }

    protected virtual void OnExit()
    {
        TweenScale(initScale);
    }

    protected virtual void TweenScale(Vector3 scale, float time = 0.15f)
    {
        DOTween.Kill(transform, true);
        transform.DOScale(scale, time);//.SetEase(Ease.OutElastic);
    }

    protected override void OnDisable()
    {
        base.OnEnable();
        DOTween.Kill(transform, true);
    }
}
