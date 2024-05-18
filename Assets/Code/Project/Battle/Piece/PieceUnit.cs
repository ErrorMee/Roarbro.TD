using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PieceUnit : WorldUnit
{
    [ReadOnlyProperty]
    public PieceInfo info;

    public TextMeshPro txt;

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