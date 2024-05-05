using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCell : ScrollCell<EnemyConfig>
{
    [SerializeField] protected TextMeshProUGUI title = default;
    [SerializeField] Graphic select = default;
    [SerializeField] Graphic diffuse = default;

    public override void UpdateContent(EnemyConfig info)
    {
        base.UpdateContent(info);

        title.text = info.avatar.Opt00Str();

        btn.targetGraphic.color = info.color;

        select.gameObject.SetActive(Index == Context.SelectedIndex);
        diffuse.gameObject.SetActive(Index != Context.SelectedIndex);
    }
}