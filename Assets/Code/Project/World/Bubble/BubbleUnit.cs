using DG.Tweening;
using EasingCore;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class BubbleUnit : MonoBehaviour
{
    enum BubbleState
    {
        Enter,
        Keep,
        Exit,
    }

    const float EnterTime = 0.2f;
    const float EnterTimeA = 1 / EnterTime;

    const float KeepTime = 1.5f;

    const float ExitTime = 0.2f;
    const float ExitTimeA = 1 / ExitTime;

    BubbleState bubbleState = BubbleState.Enter;

    public TextMeshPro txt;

    private float enterTime = EnterTime;
    private float keepTime = KeepTime;
    private float exitTime = ExitTime;

    private void Reset()
    {
        txt.text = string.Empty;
        txt.alpha = 0;
        enterTime = EnterTime;
        keepTime = KeepTime;
        exitTime = ExitTime;
        bubbleState = BubbleState.Enter;
    }

    private void Awake()
    {
        txt.alpha = 0;
    }

    private void Update()
    {
        switch (bubbleState)
        {
            case BubbleState.Enter:
                enterTime -= Time.deltaTime;
                if (enterTime <= 0)
                {
                    bubbleState = BubbleState.Keep;
                    txt.alpha = 1;
                    txt.transform.localScale = Vector3.one;
                }
                else
                {
                    transform.localPosition += new Vector3(0, 0, 0.01f);
                    txt.alpha = 1 - enterTime * EnterTimeA;
                    float scale = Easing.Get(EasingCore.Ease.OutBack).Invoke(txt.alpha);
                    txt.transform.localScale = new Vector3(scale, scale, scale);
                }
                break;
            case BubbleState.Keep:
                keepTime -= Time.deltaTime;
                transform.localPosition += new Vector3(0, 0, 0.001f);
                if (keepTime <= 0)
                {
                    bubbleState = BubbleState.Exit;
                }
                break;
            case BubbleState.Exit:
                exitTime -= Time.deltaTime;
                if (exitTime <= 0)
                {
                    Reset();
                    GameObjectPool.Instance.Cache(gameObject);
                }
                else
                {
                    txt.alpha = exitTime * ExitTimeA;
                    transform.localPosition += new Vector3(0, 0, 0.005f);
                }
                break;
        }
    }
}