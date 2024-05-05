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
        
        BattleConfig battleConfig = BattleModel.Instance.battle.config;
        TerrainConfig terrainConfig = battleConfig.GetTerrainConfig();

        //title.text = info.GetName();
        //title.color = terrainConfig.GetColor(info);

        btn.targetGraphic.color = terrainConfig.GetColor(info);

        select.gameObject.SetActive(Index == Context.SelectedIndex);
        diffuse.gameObject.SetActive(Index != Context.SelectedIndex);
    }
}