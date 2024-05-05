using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerrainCell : ScrollCell<TerrainEnum>
{
    [SerializeField] protected TextMeshProUGUI title = default;
    [SerializeField] Graphic select = default;
    [SerializeField] Graphic diffuse = default;

    public override void UpdateContent(TerrainEnum info)
    {
        base.UpdateContent(info);
        
        TerrainConfig terrainConfig = BattleModel.Instance.battle.terrain;

        //title.text = info.GetName();
        //title.color = terrainConfig.GetColor(info);

        btn.targetGraphic.color = terrainConfig.GetColor(info);

        select.gameObject.SetActive(Index == Context.SelectedIndex);
        diffuse.gameObject.SetActive(Index != Context.SelectedIndex);
    }
}