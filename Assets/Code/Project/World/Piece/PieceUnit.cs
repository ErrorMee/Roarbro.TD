using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PieceUnit : WorldUnit
{
    [ReadOnlyProperty]
    public PieceInfo info;

    public TextMeshPro txt;

    public bool fighting = false;

    private float viewRadius = 1.2f;

    static int[] xSearchs = new int[] { 0, -1, 1, 2, -2, -3, 3, 4, -4, -5, 5, 6, -6, -7, 7, 8, -8, -9, 9 };

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        FindEnemy();
    }

    private void FindEnemy()
    {
        if (fighting)
        {
            Vector2Int center = info.GetViewCoord();
            int viewRadiusInt = Mathf.RoundToInt(viewRadius);
            for (int z = -viewRadiusInt; z <= viewRadiusInt; z++)
            {
                for (int x = 0; x <= viewRadiusInt * 2; x++)
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
                            if (dis < viewRadius)
                            {
                                if (dis > 0.1f)
                                {
                                    transform.forward = new Vector3(enemyDir.x, 0, enemyDir.y);
                                }
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
        meshRenderer.SetMPBInt(MatPropUtil.IndexKey, info.type, false);

        Color color = Color.white;
        switch (info.type)
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
            if (info.level >= PieceModel.PieceMaxLV)
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