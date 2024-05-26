using System.Collections.Generic;
using UnityEngine;

public partial class BallModel : Singleton<BallModel>, IDestroy
{
    public List<BallInfo> readyMerges = new List<BallInfo>();

    public void CheckMerges()
    {
        if (dragIndexs.Count > 0)
        {
            for (int i = dragIndexs.Count - 1; i >= 0; i--)
            {
                CrossMerges(dragIndexs[i]);
                dragIndexs.RemoveAt(i);
                if (readyMerges.Count > 2)
                {
                    return;
                }
            }
        }
        for (int y = GridUtil.YMaxIndex; y >= 0; y--)
        {
            bool random = Random.Range(0, 2) == 0;
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                int randX = random ? x : GridUtil.XCount - x - 1;
                CrossMerges(new Vector2Int(randX, y));
                if (readyMerges.Count > 2)
                {
                    return;
                }
            }
        }
    }

    private void CrossMerges(Vector2Int index)
    {
        readyMerges.Clear();
        BallInfo ball0 = GetBall(index);
        if (ball0 == null)
        {
            return;
        }

        if (ball0.level < BallMaxLV)
        {
            readyMerges.Add(ball0);
        }

        int centerCount = readyMerges.Count;
        BiVectorMerge(ball0, Vector2Int.left, centerCount);
        BiVectorMerge(ball0, Vector2Int.up, centerCount);
    }

    private void BiVectorMerge(BallInfo ball0, Vector2Int offset, int centerCount)
    {
        int preCount = readyMerges.Count;
        UniVectorMerge(ball0, offset);
        UniVectorMerge(ball0, -offset);
        int addCount = readyMerges.Count - preCount;
        if ((addCount + centerCount) < 3)
        {
            readyMerges.RemoveRange(preCount, addCount);
        }
    }

    private void UniVectorMerge(BallInfo ball0, Vector2Int offset)
    {
        Vector2Int offsetAdd = offset;
        BallInfo next = GetMerge(ball0, offsetAdd);
        while (next != null)
        {
            if (next.level < BallMaxLV)
            {
                readyMerges.Add(next);
            }
            offsetAdd += offset;
            next = GetMerge(ball0, offsetAdd);
        }
    }

    private BallInfo GetMerge(BallInfo ball, Vector2Int offset)
    {
        BallInfo ballOffset = GetBall(ball.index + offset);
        if (ballOffset != null && ballOffset.config.id == ball.config.id)
        {
            return ballOffset;
        }
        return null;
    }
}