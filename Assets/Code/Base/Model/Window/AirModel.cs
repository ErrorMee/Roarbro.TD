using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AirModel : Singleton<AirModel>, IDestroy
{
    RectTransform air;
    RawImage rawImage;
    readonly LinkedList<Transform> callerList = new();
    readonly Dictionary<Transform, Action> callerCallBack = new();

    Tweener tweenFade;
    readonly Dictionary<AirEnum, Material> airMats = new();

    public Stack<AirEnum> airStack = new Stack<AirEnum>();

    public void Init()
    {
        callerList.Clear();
        callerCallBack.Clear();
        GameObject gameObject = new("AirRayCast");
        air = gameObject.GetOrAddComponent<RectTransform>();
        air.SetParent(WindowModel.Instance.gameObject.transform, false);
        air.anchorMin = Vector2.zero;
        air.anchorMax = Vector2.one;
        air.offsetMin = air.offsetMax = Vector2.zero;
        air.gameObject.layer = LayerMask.NameToLayer("UI");
        gameObject.AddComponent<CanvasRenderer>();
        rawImage = gameObject.AddComponent<RawImage>();
        rawImage.color = Color.clear;
        rawImage.texture = StaticBlurFeature.staticBlurRT;
        air.SetAsFirstSibling();
        gameObject.SetActive(false);
        ClickListener.Add(air).onClick += OnClickAir;
    }

    public static void Add(Transform caller, Action callback = null, AirEnum airEnum = AirEnum.BlurGaussian)
    {
        Instance.airStack.Push(airEnum);
        Instance.rawImage.color = Color.clear;
        WindowModel.GetLayer(WindowLayerEnum.Bot).canvasGroup.alpha = 1;
        switch (airEnum)
        {
            case AirEnum.Alpha: break;
            default:
                if (!Instance.airMats.TryGetValue(airEnum, out Material material))
                {
                    material = AddressModel.LoadMaterial(Address.BgMaterial(airEnum.ToString()));
                    Instance.airMats.Add(airEnum, material);
                }

                StaticBlurFeature.Instance.Show(material);
                break;
        }

        Instance.air.SetParent(caller.parent, false);
        if (Instance.air.gameObject.activeInHierarchy && Instance.air.parent == caller.parent)
        {
            Instance.air.SetSiblingIndex(caller.GetSiblingIndex() - 1);
        }
        else
        {
            Instance.air.SetSiblingIndex(caller.GetSiblingIndex());
        }
        
        if (Instance.callerList.Contains(caller))
        {
            Instance.callerList.Remove(caller);
        }
        Instance.callerList.AddLast(caller);
        Instance.callerCallBack[caller] = callback;

        float progress = 0;
        Instance.tweenFade = DOTween.To(() => progress, x => progress = x, 1, 0.15f);
        Instance.tweenFade.SetDelay(0.05f);
        Instance.tweenFade.onUpdate = () => {
            Instance.rawImage.color = new Color(1, 1, 1, progress);
            WindowModel.GetLayer(WindowLayerEnum.Bot).canvasGroup.alpha = 1 - progress;
        };

        Instance.air.gameObject.SetActive(true);
    }

    public static void Remove(Transform caller)
    {
        if (Boot.isQuiting)
        {
            return;
        }
        Instance.airStack.Pop();
        Instance.tweenFade.Complete(true);

        Instance.callerCallBack.Remove(caller);
        Instance.callerList.Remove(caller);
        if (Instance.callerList.Count > 0)
        {
            Instance.air.SetParent(Instance.callerList.Last.Value.parent, false);
            Instance.air.SetSiblingIndex(Instance.callerList.Last.Value.GetSiblingIndex());
        }
        else
        {
            Instance.air.SetParent(WindowModel.Instance.gameObject.transform, false);
            Instance.air.SetAsFirstSibling();
            Instance.rawImage.color = Color.clear;
            Instance.air.gameObject.SetActive(false);
            WindowModel.GetLayer(WindowLayerEnum.Bot).canvasGroup.alpha = 1;
        }
    }

    void OnClickAir()
    {
        if (callerList.Count > 0)
        {
            Transform callerLast = callerList.Last.Value;
            callerCallBack.TryGetValue(callerLast, out Action callback);

            if (callback != null)
            {
                callback?.Invoke();
            }
        }
    }
}