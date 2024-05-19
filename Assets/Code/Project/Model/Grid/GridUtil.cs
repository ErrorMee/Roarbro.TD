using System.Collections.Generic;
using UnityEngine;

public static class GridUtil
{
    public const int XRadiusCount = 3;

    public const int YRadiusCount = 3;

    public const int XMaxIndex = XRadiusCount * 2;
    public const int XCount = XMaxIndex + 1;

    public const int YMaxIndex = YRadiusCount * 2;
    public const int YCount = YMaxIndex + 1;

    public const int XAddYCount = XCount + YCount;

    public const int AllCount = XCount * YCount;

    public const int FightStartY = YRadiusCount + 1 + 4;
    public const int FightEndY = -YRadiusCount - 1 - 1;

    public static Vector2Int WorldToGridIndex(Vector3 worldPos)
    {
        Vector2Int gridIndex = new Vector2Int(Mathf.RoundToInt(worldPos.x) + XRadiusCount,
            Mathf.RoundToInt(worldPos.z) + YRadiusCount);
        return gridIndex;
    }

    public static Vector3 WorldToGridPos(Vector3 worldPos, bool justInGrid)
    {
        Vector3 gridPos = new Vector3(Mathf.RoundToInt(worldPos.x),
           worldPos.y, Mathf.RoundToInt(worldPos.z));
        if (justInGrid)
        {
            if (Mathf.Abs(gridPos.x) > XRadiusCount || Mathf.Abs(gridPos.z) > YRadiusCount)
            {
                gridPos.x = short.MaxValue;
            }
        }
        return gridPos;
    }

    public static bool InGrid(int indexx, int indexy)
    {
        if (indexx >= 0 && indexx < XCount)
        {
            if (indexy >= 0 && indexy < YCount)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsCorner(int indexx, int indexy)
    {
        if ((indexx % XMaxIndex == 0) && (indexy % YMaxIndex == 0))
        {
            return true;
        }
        return false;
    }

    public static bool IsEdge(int indexx, int indexy)
    {
        if ((indexx % XMaxIndex == 0) || (indexy % YMaxIndex == 0))
        {
            return true;
        }
        return false;
    }

    public static HashSet<Vector2Int> GetEdgePoss()
    {
        HashSet<Vector2Int> edgePoss = new HashSet<Vector2Int>();
        for (int x = 0; x < XCount; x++)
        {
            edgePoss.Add(new Vector2Int(x, 0));
            edgePoss.Add(new Vector2Int(x, YMaxIndex));
        }
        for (int y = 1; y < YCount - 1; y++)
        {
            edgePoss.Add(new Vector2Int(0, y));
            edgePoss.Add(new Vector2Int(XMaxIndex, y));
        }
        return edgePoss;
    }

    public static int GetIndex(int indexx, int indexy)
    {
        return indexx + indexy * XCount;
    }
}