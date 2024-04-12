using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageCell : ScrollCell<LanguageType>
{
    [SerializeField] TextMeshProUGUI title = default;
    [SerializeField] Graphic select = default;

    public override void UpdateContent(LanguageType info)
    {
        base.UpdateContent(info);
        title.text = info.tag;
        select.gameObject.SetActive(Index == Context.SelectedIndex);
    }
}