using System;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyLayer : BattleLayer<EnemyUnit>
{
    public static bool Edit = false;

    private void Update()
    {
        if (Edit && InputModel.Instance.Presseding)
        {
            OnPresseding();
        }
    }

    private void OnPresseding()
    {
        Vector3 worldPos = CameraModel.Instance.ScreenToWorldPos(InputModel.Instance.Touch0LastPos, transform.position.y);
        worldPos = GridUtil.WorldToGridPos(worldPos, true);
        select.transform.position = worldPos;
        select.gameObject.SetActive(true);

        Vector2Int index = GridUtil.WorldToGridIndex(worldPos);

        if (GridUtil.InGrid(index.x, index.y))
        {
            int idx = GridUtil.GetIndex(index.x, index.y);
            EnemyInfoConfig[] enemyInfoConfigs = BattleModel.Instance.battle.army.enemys;
            EnemyInfoConfig enemyInfoConfig = enemyInfoConfigs[idx];

            if (enemyInfoConfig.enemyID != EnemyModel.Instance.selectEnemy.enemyID
                || enemyInfoConfig.level != EnemyModel.Instance.selectEnemy.level)
            {
                enemyInfoConfig.enemyID = EnemyModel.Instance.selectEnemy.enemyID;
                enemyInfoConfig.level = EnemyModel.Instance.selectEnemy.level;
                EnemyModel.Instance.infos[index.x, index.y].SetEnemyInfoConfig(enemyInfoConfig);

                ArmyConfigs.Instance.Save();
                UpdateUnits();
            }
        }
    }
}