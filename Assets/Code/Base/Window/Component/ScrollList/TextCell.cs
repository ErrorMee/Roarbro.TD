using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextCell : ScrollCell<string>
{
    [SerializeField] protected TextMeshProUGUI title = default;
    [SerializeField] Graphic select = default;
    public override void UpdateContent(string info)
    {
        base.UpdateContent(info);
        title.text = info;
        if (select != null)
        {
            select.gameObject.SetActive(Index == Context.SelectedIndex);
        }
    }
}
