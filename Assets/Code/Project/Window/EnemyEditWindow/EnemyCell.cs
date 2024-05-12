using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCell : ScrollCell<EnemyConfig>
{
    [SerializeField] protected TextMeshProUGUI title = default;

    [SerializeField] SDFImg shadow = default;
    [SerializeField] SDFImg bg = default;

    [SerializeField] SDFImg select = default;
    [SerializeField] SDFImg diffuse = default;

    public override void UpdateContent(EnemyConfig info)
    {
        base.UpdateContent(info);

        //if (info.avatar <= 0)
        {
            title.text = string.Empty;
        }
        //else
        //{
        //    title.text = info.avatar.Opt00Str();
        //}

        shadow.ID = bg.ID = select.ID = diffuse.ID = info.avatar;

        btn.targetGraphic.color = info.color;

        select.gameObject.SetActive(Index == Context.SelectedIndex);
        diffuse.gameObject.SetActive(Index != Context.SelectedIndex);
    }
}