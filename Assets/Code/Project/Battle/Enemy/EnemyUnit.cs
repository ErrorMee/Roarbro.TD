using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyUnit : BattleUnit
{
    [ReadOnlyProperty]
    public EnemyInfo info;
    public MeshRenderer shadow;
    public TextMeshPro txt;

    protected override void OnEnable() { }
    protected override void OnDisable() { }

    public void UpdateShow()
    {
        meshRenderer.SetMPBInt(MatPropUtil.IndexKey, info.config.avatar, false);
        meshRenderer.SetMPBColor(MatPropUtil.BaseColorKey, info.config.color);

        shadow.gameObject.SetActive(info.config.id > 0);
        if (info.config.id > 0)
        {
            txt.text = info.level.OptStr();
        }
        else
        {
            txt.text = "";
        }
    }
}