using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class BallUnit : MonoBehaviour
{
    static int[] xSearchs = new int[] { 0, -1, 1, 2, -2, -3, 3, 4, -4, -5, 5, 6, -6, -7, 7, 8, -8, -9, 9 };

    private void FightEnter()
    {
    }

    private void FightUpdate()
    {
        info.attCDLeft -= Time.fixedDeltaTime;

        if (info.attCDLeft <= 0)
        {
            info.attCDLeft += info.config.attCD;
            TryFight();
        }
    }

    private void TryFight()
    {
        Vector2Int center = info.GetViewCoord();
        int attRadiusInt = Mathf.RoundToInt(info.config.attRadius);
        for (int z = -attRadiusInt; z <= attRadiusInt; z++)
        {
            for (int x = 0; x <= attRadiusInt * 2; x++)
            {
                int xSearch = xSearchs[x];

                Vector2Int coord = center + new Vector2Int(xSearch, z);

                HashSet<MonoBehaviour> enemys = GridModel.Instance.GetItems<EnemyUnit>(coord);
                if (enemys != null)
                {
                    foreach (MonoBehaviour item in enemys)
                    {
                        Vector2 enemyDir = item.transform.localPosition.XZ() - transform.localPosition.XZ();
                        float dis = enemyDir.magnitude;
                        if (dis < info.config.attRadius)
                        {
                            Emit(item as EnemyUnit);
                            return;
                        }
                    }
                }
            }
        }
    }

    private void Emit(EnemyUnit enemyUnit)
    {
        BulletInfo bulletInfo = SharedPool<BulletInfo>.Get();
        bulletInfo.ballUnit = this;
        bulletInfo.enemyUnit = enemyUnit;
        EventModel.Send(EventEnum.BulletUnit, bulletInfo);

        //enemyUnit.Attacked(info);
    }
}