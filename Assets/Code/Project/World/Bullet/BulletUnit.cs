using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletUnit : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    private BulletInfo info;

    private float leftLife;

    private Vector3 deltaDir;

    public void Init(BulletInfo _info)
    {
        info = _info;

        transform.localScale = Vector3.one * info.ballUnit.info.bulletConfig.radius * 2;

        transform.localPosition = info.ballUnit.transform.localPosition;

        meshRenderer.SetMPBInt(MatPropUtil.IndexKey, info.ballUnit.info.bulletConfig.id, false);

        meshRenderer.SetMPBColor(MatPropUtil.BaseColorKey, info.ballUnit.info.config.color);

        leftLife = info.ballUnit.info.bulletConfig.life;

        Vector3 dir = (info.enemyUnit.transform.localPosition - info.ballUnit.transform.localPosition).normalized;
        deltaDir = dir * Time.fixedDeltaTime;
        transform.forward = dir;
    }    

    void FixedUpdate()
    {
        if (BattleModel.Instance.pause == false)
        {
            leftLife -= Time.fixedDeltaTime;

            if (leftLife <= 0)
            {
                leftLife -= Time.fixedDeltaTime * 2;

                Color color = info.ballUnit.info.config.color;

                color.a = 1 + leftLife;

                if (color.a <= 0)
                {
                    GameObjectPool.Instance.Cache(gameObject);
                    return;
                }
                meshRenderer.SetMPBInt(MatPropUtil.IndexKey, info.ballUnit.info.bulletConfig.id, false);
                meshRenderer.SetMPBColor(MatPropUtil.BaseColorKey, color);
                return;
            }

            transform.localPosition += deltaDir * info.ballUnit.info.bulletConfig.speed;

            TryFight();
        }
    }

    private void TryFight()
    {
        Vector2Int center = new Vector2Int(Mathf.RoundToInt(transform.localPosition.x),
            Mathf.RoundToInt(transform.localPosition.z));

        int attRadiusInt = Mathf.RoundToInt(info.ballUnit.info.bulletConfig.radius);
        for (int z = -attRadiusInt; z <= attRadiusInt; z++)
        {
            for (int x = 0; x <= attRadiusInt * 2; x++)
            {
                int xSearch = GridModel.XSearchs[x];

                Vector2Int coord = center + new Vector2Int(xSearch, z);

                HashSet<MonoBehaviour> enemys = GridModel.Instance.GetItems<EnemyUnit>(coord);
                if (enemys != null)
                {
                    foreach (MonoBehaviour item in enemys)
                    {
                        EnemyUnit enemy = item as EnemyUnit;
                        Vector2 enemyDir = enemy.transform.localPosition.XZ() - transform.localPosition.XZ();
                        float dis = enemyDir.magnitude;
                        if (dis < info.ballUnit.info.bulletConfig.radius + enemy.info.config.radius)
                        {
                            enemy.Attacked(info.ballUnit.info);
                            GameObjectPool.Instance.Cache(gameObject);
                            return;
                        }
                    }
                }
            }
        }
    }
}