using System;
using System.Collections.Generic;
using UnityEngine;

public class PieceInfo
{
    public int type;

    public int level;

    public Vector2Int index;

    public float GetViewX()
    {
        return index.x - GridUtil.XRadiusCount;
    }

    public float GetViewZ()
    {
        return index.y - GridUtil.YRadiusCount;
    }
}