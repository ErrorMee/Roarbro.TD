using DG.Tweening;
using UnityEngine;

public class SplashWindow : WindowBase
{
    public CanvasGroup canvasGroup;

    [Range(0, 1)]
    public float enterSize = 0.9f;
    public float enterTime = 1.5f;
    public Ease enterEase = Ease.OutSine;

    public float stopTime = 2.5f;
    public float outTime = 0.5f;
    public Ease outEase = Ease.InSine;

    public float endTime = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 0;
        canvasGroup.transform.localScale = Vector3.one * enterSize;
    }

    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);

        canvasGroup.transform.DOScale(Vector3.one, enterTime).SetEase(enterEase);
        canvasGroup.DOFade(1, enterTime).SetEase(enterEase);
        canvasGroup.DOFade(0, outTime).SetEase(outEase).SetDelay(enterTime + stopTime);

        DOVirtual.DelayedCall(enterTime + stopTime + outTime + endTime, () =>
        {
            CloseSelf();
            Open(WindowEnum.Lobby);
        });
    }
}