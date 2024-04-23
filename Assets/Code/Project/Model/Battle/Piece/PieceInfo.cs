using UnityEngine;

public class PieceInfo
{
    public int type;

    public int level = 1;

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

    public int GetMatchPriority(PieceInfo start)
    {
        // level distance random
        Vector2Int offsetIndex = index - start.index;
        int distance = Mathf.Max(Mathf.Abs(offsetIndex.x), Mathf.Abs(offsetIndex.y));
        return level * 1000 + (20 - distance) * 10 + Random.Range(0, 10);
    }
}