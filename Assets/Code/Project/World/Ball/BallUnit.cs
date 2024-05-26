using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallUnit : WorldUnit
{
    [ReadOnlyProperty]
    public BallInfo info;

    public TextMeshPro txt;

    static int[] xSearchs = new int[] { 0, -1, 1, 2, -2, -3, 3, 4, -4, -5, 5, 6, -6, -7, 7, 8, -8, -9, 9 };

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        FindEnemy();
    }

    private void FindEnemy()
    {
        if (BallModel.Instance.fighting)
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
                                //if (dis > 0.1f)
                                //{
                                //    transform.forward = new Vector3(enemyDir.x, 0, enemyDir.y);
                                //}
                                return;
                            }
                        }
                    }
                }
            }
        }
    }

    public void UpdateShow()
    {
        meshRenderer.SetMPBInt(MatPropUtil.IndexKey, info.config.id, false);

        Color color = Color.white;
        switch (info.config.id)
        {
            case 1:
                color = QualityConfigs.GetColor(QualityEnum.ER);
                break;
            case 2:
                color = QualityConfigs.GetColor(QualityEnum.UR);
                break;
            case 3:
                color = QualityConfigs.GetColor(QualityEnum.R);
                break;
            case 4:
                color = QualityConfigs.GetColor(QualityEnum.SR);
                break;
            case 5:
                color = QualityConfigs.GetColor(QualityEnum.SSR);
                break;
        }

        meshRenderer.SetMPBColor(MatPropUtil.BaseColorKey, color);
        if (info.level > 1)
        {
            if (info.level >= BallModel.BallMaxLV)
            {
                txt.text = "Max";
            }
            else
            {
                txt.text = info.level.OptStr();
            }
        }
        else
        {
            txt.text = String.Empty;
        }
    }
}