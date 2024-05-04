using System.Collections.Generic;
using UnityEngine;

public partial class PieceModel : Singleton<PieceModel>, IDestroy
{
    public List<PieceInfo> readyMerges = new List<PieceInfo>();

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
        PieceInfo piece0 = GetPiece(index);
        if (piece0 == null)
        {
            return;
        }

        if (piece0.level < PieceMaxLV)
        {
            readyMerges.Add(piece0);
        }

        int centerCount = readyMerges.Count;
        BiVectorMerge(piece0, Vector2Int.left, centerCount);
        BiVectorMerge(piece0, Vector2Int.up, centerCount);
    }

    private void BiVectorMerge(PieceInfo piece0, Vector2Int offset, int centerCount)
    {
        int preCount = readyMerges.Count;
        UniVectorMerge(piece0, offset);
        UniVectorMerge(piece0, -offset);
        int addCount = readyMerges.Count - preCount;
        if ((addCount + centerCount) < 3)
        {
            readyMerges.RemoveRange(preCount, addCount);
        }
    }

    private void UniVectorMerge(PieceInfo piece0, Vector2Int offset)
    {
        Vector2Int offsetAdd = offset;
        PieceInfo next = GetMerge(piece0, offsetAdd);
        while (next != null)
        {
            if (next.level < PieceMaxLV)
            {
                readyMerges.Add(next);
            }
            offsetAdd += offset;
            next = GetMerge(piece0, offsetAdd);
        }
    }

    private PieceInfo GetMerge(PieceInfo piece, Vector2Int offset)
    {
        PieceInfo pieceOffset = GetPiece(piece.index + offset);
        if (pieceOffset != null && pieceOffset.type == piece.type)
        {
            return pieceOffset;
        }
        return null;
    }
}