using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletLayer : WorldLayer<BulletUnit>
{
    protected override void Awake()
    {
        base.Awake();
        AutoListener(EventEnum.BulletUnit, OnBulletUnit);
    }

    void OnBulletUnit(object obj)
    {
        BulletInfo bulletInfo = obj as BulletInfo;

        BulletUnit bulletUnit = GameObjectPool.Instance.Get<BulletUnit>(unitTemplate.gameObject);
        bulletUnit.Init(bulletInfo);
    }
}