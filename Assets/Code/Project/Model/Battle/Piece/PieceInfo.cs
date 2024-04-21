using System;
using System.Collections.Generic;
using UnityEngine;

public class PieceInfo
{
    public int type;

    public int level = 0;

    public Vector2Int index;

    public float GetViewX()
    {
        return index.x - GridUtil.XRadiusCount;
    }

    public float GetViewZ()
    {
        return index.y - GridUtil.YRadiusCount;
    }

    public bool DeleteMark
    {
        set;get;
    }


}