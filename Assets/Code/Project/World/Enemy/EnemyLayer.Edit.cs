using System;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyLayer : WorldLayer<EnemyUnit>
{
    private void EditEnter()
    {
        CreateSelect();

        units = new EnemyUnit[GridUtil.XCount, GridUtil.YCount];

        UpdateEditUnits();
    }

    private void UpdateEditUnits()
    {
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                EnemyUnit unit = units[x, y];
                if (unit == null)
                {
                    unit = CreateUnit();
                    units[x, y] = unit;
                    unit.info = EnemyModel.Instance.infos[x, y]; ;
                }
                unit.UpdateShow();
                unit.transform.localPosition = new Vector3(x - GridUtil.XRadiusCount, 0, y - GridUtil.YRadiusCount);
                unit.fsm.ChangeState(EnemyState.Idle);
            }
        }
    }

    private void EditUpdate()
    {
        if (InputModel.Instance.Presseding)
        {
            Vector3 worldPos = CameraModel.Instance.ScreenToWorldPos(InputModel.Instance.Touch0LastPos, transform.position.y);
            worldPos = GridUtil.WorldToGridPos(worldPos, true);
            select.transform.position = worldPos;
            select.gameObject.SetActive(true);

            Vector2Int index = GridUtil.WorldToGridIndex(worldPos);

            if (GridUtil.InGrid(index.x, index.y))
            {
                int idx = GridUtil.GetIndex(index.x, index.y);
                EnemyTemplate[] enemyInfoConfigs = BattleModel.Instance.battle.army.enemys;
                EnemyTemplate enemyInfoConfig = enemyInfoConfigs[idx];

                if (enemyInfoConfig.Equal(EnemyModel.Instance.crtTemplate) == false)
                {
                    enemyInfoConfig.enemyID = EnemyModel.Instance.crtTemplate.enemyID;
                    enemyInfoConfig.level = EnemyModel.Instance.crtTemplate.level;
                    EnemyModel.Instance.infos[index.x, index.y].SetEnemyTemplate(enemyInfoConfig);

                    ArmyConfigs.Instance.Save();
                    UpdateEditUnits();
                }
            }
        }
    }

}