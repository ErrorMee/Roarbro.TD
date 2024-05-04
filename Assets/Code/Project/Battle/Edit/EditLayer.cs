using DG.Tweening;
using UnityEngine;

public class EditLayer : BattleLayer<UnitSelect>
{
    UnitSelect unit;

    public static bool Start = false;

    protected override void Awake()
    {
        base.Awake();
        unit = CreateUnit();
        unit.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Start && InputModel.Instance.Presseding)
        {
            OnPresseding();
        }
    }

    private void OnPresseding()
    {
        Vector3 worldPos = CameraModel.Instance.ScreenToWorldPos(InputModel.Instance.Touch0LastPos, transform.position.y);
        worldPos = GridUtil.WorldToGridPos(worldPos, true);
        Vector2Int index = GridUtil.WorldToGridIndex(worldPos);

        unit.transform.position = worldPos;
        unit.gameObject.SetActive(true);

        if (GridUtil.InGrid(index.x, index.y))
        {
            int idx = GridUtil.GetIndex(index.x, index.y);
            if (BattleModel.Instance.battle.edit)
            {
                TerrainEnum[] terrains = BattleModel.Instance.battle.config.GetTerrains();
                if (terrains[idx] != BattleModel.Instance.battle.config.terrainSelect)
                {
                    terrains[idx] = BattleModel.Instance.battle.config.terrainSelect;
                    BattleConfigs.Instance.Save();
                    EventModel.Send(EventEnum.ResetTerrain);
                }
            }
        }
    }
}