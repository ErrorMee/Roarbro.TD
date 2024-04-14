using System;
using System.Collections.Generic;
using UnityEngine;

public class PieceInfo
{
    public int type;

    public int level;

    public int posx;

    public int posy;

    public float GetViewX()
    {
        return posx - GridUtil.XRadiusCount;
    }

    public float GetViewZ()
    {
        return posy - GridUtil.YRadiusCount;
    }
}