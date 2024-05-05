using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageCell : ScrollCell<BattleInfo>
{
    [SerializeField] TextMeshProUGUI title = default;
    [SerializeField] Graphic select = default;
    [SerializeField] Graphic diffuse = default;
    [SerializeField] Graphic icon = default;

    public override void UpdateContent(BattleInfo info)
    {
        base.UpdateContent(info);
        title.text = (info.config.id).ToString();
        select.gameObject.SetActive(Index == Context.SelectedIndex);
        diffuse.gameObject.SetActive(Index != Context.SelectedIndex);
        btn.interactable = StageModel.Instance.IsUnLock(Index);
    }
}