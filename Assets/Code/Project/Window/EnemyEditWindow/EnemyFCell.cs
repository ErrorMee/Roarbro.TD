using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyFCell : ScrollFocusCell<EnemyTemplate>
{
    [SerializeField] protected TextMeshProUGUI title = default;

    [SerializeField] SDFImg shadow = default;
    [SerializeField] SDFImg bg = default;
    [SerializeField] SDFImg diffuse = default;

    public override void UpdateContent(EnemyTemplate info)
    {
        base.UpdateContent(info);

        EnemyConfig enemyConfig = EnemyConfigs.ConfigByID(info.enemyID);

        //if (enemyConfig.avatar <= 0)
        {
            title.text = string.Empty;
        }
        //else
        //{
        //    title.text = info.level.Opt00Str();
        //}

        shadow.ID = bg.ID = select.ID = diffuse.ID = enemyConfig.avatar;

        btn.targetGraphic.color = enemyConfig.color;

        diffuse.gameObject.SetActive(Index != Context.SelectedIndex);
    }
}