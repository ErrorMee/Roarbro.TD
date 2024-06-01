using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageFCell : ScrollFocusCell<BattleInfo>
{
    [SerializeField] TextMeshProUGUI title = default;
    [SerializeField] Graphic diffuse = default;

    public override void UpdateContent(BattleInfo info)
    {
        base.UpdateContent(info);
        title.text = (info.config.id).ToString();
        diffuse.gameObject.SetActive(Index != Context.SelectedIndex);
        btn.interactable = StageModel.Instance.IsUnLock(Index);
    }
}