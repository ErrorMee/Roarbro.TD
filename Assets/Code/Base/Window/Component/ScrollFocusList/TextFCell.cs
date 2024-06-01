using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFCell : ScrollFocusCell<string>
{
    [SerializeField] protected TextMeshProUGUI title = default;

    public override void UpdateContent(string info)
    {
        base.UpdateContent(info);
        title.text = info;
    }
}
