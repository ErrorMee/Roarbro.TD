using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class BubbleUnit : MonoBehaviour
{
    const float EnterTime = 0.5f;
    const float EnterTimeA = 1 / EnterTime;
    const float ExitTime = 0.5f;
    const float ExitTimeA = 1 / ExitTime;

    public TextMeshPro txt;

    private float enterTime = EnterTime;

    private float exitTime = ExitTime;

    private void Reset()
    {
        txt.text = string.Empty;
        txt.alpha = 0;
        enterTime = EnterTime;
        exitTime = ExitTime;
    }

    private void Awake()
    {
        txt.alpha = 0;
    }

    private void Update()
    {
        enterTime -= Time.deltaTime;

        if (enterTime < 0)
        {
            exitTime -= Time.deltaTime;

            txt.alpha = exitTime * ExitTimeA;

            if (exitTime < 0)
            {
                Reset();
                GameObjectPool.Instance.Cache(gameObject);
            }
        }
        else
        {
            txt.alpha = 1 - enterTime * EnterTimeA;
        }
    }
}