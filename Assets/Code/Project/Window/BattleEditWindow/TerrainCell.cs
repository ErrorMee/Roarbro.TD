using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerrainCell : ScrollCell<TerrainEnum>
{
    [SerializeField] protected TextMeshProUGUI title = default;
    [SerializeField] Graphic select = default;
    public override void UpdateContent(TerrainEnum info)
    {
        base.UpdateContent(info);
        title.text = info.GetName();
        title.color = info.GetColor();
        if (select != null)
        {
            select.gameObject.SetActive(Index == Context.SelectedIndex);
        }
    }
}