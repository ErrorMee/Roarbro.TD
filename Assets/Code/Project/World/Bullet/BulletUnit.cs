using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletUnit : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    private BulletInfo info;

    

    public void Init(BulletInfo _info)
    {
        info = _info;

        transform.localScale = Vector3.one * info.ballUnit.info.bulletConfig.size;

        transform.localPosition = info.ballUnit.transform.localPosition;

        meshRenderer.SetMPBInt(MatPropUtil.IndexKey, info.ballUnit.info.bulletConfig.id, false);

        meshRenderer.SetMPBColor(MatPropUtil.BaseColorKey, info.ballUnit.info.config.color);
    }    

    void FixedUpdate()
    {
        if (BattleModel.Instance.pause == false)
        {
            transform.localPosition += new Vector3(0, 0, Time.fixedDeltaTime * info.ballUnit.info.bulletConfig.speed);
        }
    }
}