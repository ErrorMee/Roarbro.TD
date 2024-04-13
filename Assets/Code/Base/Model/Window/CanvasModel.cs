using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
[RequireComponent(typeof(CanvasGroup))]
public class CanvasModel : SingletonBehaviour<CanvasModel>
{
    public static Vector2 designSize = new(828, 1792);
    public static Vector2 designHalfSize = designSize * 0.5f;
    [ReadOnlyProperty]
    public float designWHRate;

    [ReadOnlyProperty]
    public Vector2 phoneSize;
    [ReadOnlyProperty]
    public Rect phoneRect;
    [ReadOnlyProperty]
    public float phoneWHRate;

    [ReadOnlyProperty]
    public Vector2 phoneCenter;

    [ReadOnlyProperty]
    public float phoneToDesignRate;

    [ReadOnlyProperty]
    public Vector2 canvasSize;

    CanvasScaler canvasScaler;
    CanvasGroup canvasGroup;

    override protected void Awake()
    {
        base.Awake();
        canvasScaler = GetComponent<CanvasScaler>();
        canvasGroup = GetComponent<CanvasGroup>();

        designWHRate = designSize.x / designSize.y;
        canvasGroup.alpha = 1;
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = designSize;
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        canvasScaler.referencePixelsPerUnit = 100;

        phoneSize = new Vector2Int(Screen.width, Screen.height);
        phoneRect = new Rect(0, 0, Screen.width, Screen.height);
        phoneCenter = phoneSize * 0.5f;
        phoneWHRate = phoneSize.x / phoneSize.y;

        if (phoneWHRate > designWHRate)//手机更宽：高度不变，宽度拉长
        {
            canvasScaler.matchWidthOrHeight = 1;
            canvasSize.x = designSize.x * (phoneWHRate / designWHRate);
            canvasSize.y = designSize.y;

            phoneToDesignRate = designSize.x / phoneSize.x;
        }
        else//手机更高：宽度不变，高度拉长
        {
            canvasScaler.matchWidthOrHeight = 0;
            canvasSize.x = designSize.x;
            canvasSize.y = designSize.y * (designWHRate / phoneWHRate);

            phoneToDesignRate = designSize.y / phoneSize.y;
        }
    }

    Tweener tweenFade;
    public void FadeOut(float time, Action callBack = null)
    {
        tweenFade?.Kill();
        float progress = 1;
        canvasGroup.alpha = progress;
        canvasGroup.interactable = false;
        tweenFade = DOTween.To(() => progress, x => progress = x, 0, time);
        tweenFade.onUpdate = () => { canvasGroup.alpha = progress; };
        tweenFade.onComplete = () => { canvasGroup.interactable = true; callBack?.Invoke(); };
    }

    public void FadeIn(float time, Action callBack = null)
    {
        tweenFade?.Kill();
        float progress = 0;
        canvasGroup.alpha = progress;
        canvasGroup.interactable = false;
        tweenFade = DOTween.To(() => progress, x => progress = x, 1, time);
        tweenFade.onUpdate = () => { canvasGroup.alpha = progress; };
        tweenFade.onComplete = () => { canvasGroup.interactable = true; callBack?.Invoke(); };
    }

    public void Fade(float time = 0.15f, Action callBack = null)
    {
        FadeOut(time, () =>
        {
            callBack?.Invoke();
            FadeIn(time);
        });
    }

    public bool InScreen(Vector3 worldPos)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        return phoneRect.Contains(screenPos);
    }

    public Vector2 WorldToCanvasPos(Vector3 worldPos)
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(worldPos);

        Vector2 viewportPosRelative = new(viewportPos.x - 0.5f, viewportPos.y - 0.5f);

        return new(viewportPosRelative.x * designSize.x, viewportPosRelative.y * designSize.y);
    }

    public Vector3 ScreenToCanvasPos(Vector2 screenPos)
    {
        Vector3 viewportPos = Camera.main.ScreenToViewportPoint(screenPos);

        Vector2 viewportPosRelative = new(viewportPos.x - 0.5f, viewportPos.y - 0.5f);

        return new(viewportPosRelative.x * designSize.x, viewportPosRelative.y * designSize.y, transform.position.z);
    }

    public Vector3 CanvasToScreenPos(Vector3 canvasPos)
    {
        Vector2 viewportPosRelative = new(canvasPos.x / designSize.x, canvasPos.y / designSize.y);

        Vector3 viewportPos = new(viewportPosRelative.x + 0.5f, viewportPosRelative.y + 0.5f);

        return Camera.main.ViewportToScreenPoint(viewportPos);
    }
}
