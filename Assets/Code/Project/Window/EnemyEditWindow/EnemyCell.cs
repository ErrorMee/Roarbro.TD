using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCell : ScrollCell<EnemyConfig>
{
    [SerializeField] protected TextMeshProUGUI title = default;
    [SerializeField] Graphic select = default;
    public override void UpdateContent(EnemyConfig info)
    {
        base.UpdateContent(info);

        title.text = info.avatar.Opt00Str();

        btn.targetGraphic.color = info.color;

        if (select != null)
        {
            select.gameObject.SetActive(Index == Context.SelectedIndex);
        }
    }
}