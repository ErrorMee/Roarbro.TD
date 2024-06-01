using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageFCell : ScrollFocusCell<BattleInfo>
{
    [SerializeField] TextMeshProUGUI title = default;
    [SerializeField] Graphic diffuse = default;
    [SerializeField] SDFImg bg = default;
    [SerializeField] SDFImg terrainTemp = default;
    SDFImg[,] terrains;


    private void InitTerrains()
    {
        if (terrains == null)
        {
            terrains = new SDFImg[GridUtil.XCount, GridUtil.YCount];
            terrainTemp.gameObject.SetActive(true);
            for (int y = 0; y < GridUtil.YCount; y++)
            {
                for (int x = 0; x < GridUtil.XCount; x++)
                {
                    SDFImg item = Instantiate(terrainTemp, terrainTemp.transform.parent);
                    terrains[x, y] = item;
                    item.ID = 12;
                    item.transform.localPosition = 
                        new Vector3(x - GridUtil.XRadiusCount, y - GridUtil.YRadiusCount, 0) * item.rectTransform.sizeDelta.x;
                }
            }

            terrains[GridUtil.XCount - 1, GridUtil.YCount - 1].ID = terrains[0, 0].ID =
                terrains[GridUtil.XCount - 1, 0].ID = terrains[0, GridUtil.YCount - 1].ID = 6;

            terrains[0, GridUtil.YCount - 1].transform.Rotate(new Vector3(0, 0, 90));
            terrains[0, 0].transform.Rotate(new Vector3(0, 0, 180));
            terrains[GridUtil.XCount - 1, 0].transform.Rotate(new Vector3(0, 0, 270));

            terrainTemp.gameObject.SetActive(false);
        }
    }

    public override void UpdateContent(BattleInfo info)
    {
        base.UpdateContent(info);
        TerrainConfig terrainConfig = info.terrain;

        InitTerrains();

        title.text = (info.config.id).ToString();
        diffuse.gameObject.SetActive(Index != Context.SelectedIndex);

        bool isUnlock = StageModel.Instance.IsUnLock(Index);

        //bg.color = terrainConfig.GetColor(TerrainEnum.Water);
        btn.interactable = isUnlock;

        if (isUnlock == false)
        {
            title.color = QualityConfigs.GetColor2(QualityEnum.SSR);
        }
        else
        {
            title.color = QualityConfigs.GetColor(QualityEnum.N);
        }

        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                SDFImg item = terrains[x, y];
                TerrainEnum terrain = terrainConfig.GetTerrain(x, y);
                item.color = terrainConfig.GetColor(terrain);
            }
        }
    }
}