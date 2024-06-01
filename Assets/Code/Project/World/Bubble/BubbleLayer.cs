using System;
using System.Collections.Generic;
using UnityEngine;

public class BubbleLayer : WorldLayer<BubbleUnit>
{
    protected override void Awake()
    {
        base.Awake();
        AutoListener(EventEnum.BubbleUnit, OnBubbleUnit);
    }

    void OnBubbleUnit(object obj)
    {
        BubbleInfo bubbleInfo = obj as BubbleInfo;

        BubbleUnit bubbleUnit = GameObjectPool.Instance.Get<BubbleUnit>(unitTemplate.gameObject);

        float randomX = UnityEngine.Random.Range(-0.25f, 0.25f);

        bubbleUnit.transform.localPosition = new Vector3(bubbleInfo.x + randomX, 0, bubbleInfo.z + 0.25f);
        bubbleUnit.txt.text = bubbleInfo.value.OptStr();
        bubbleUnit.txt.color = Color.yellow;
        SharedPool<BubbleInfo>.Cache(bubbleInfo);
    }
}