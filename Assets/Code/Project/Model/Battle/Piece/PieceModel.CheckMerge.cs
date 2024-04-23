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
                CheckMerges(dragIndexs[i]);
                dragIndexs.RemoveAt(i);
                if (readyMerges.Count > 2)
                {
                    return;
                }
            }
        }
        for (int y = GridUtil.YMaxIndex; y >= 0; y--)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                CheckMerges(new Vector2Int(x, y));
                if (readyMerges.Count > 2)
                {
                    return;
                }
            }
        }
    }

    public void CheckMerges(Vector2Int index)
    {
        readyMerges.Clear();
        PieceInfo piece0 = GetPiece(index);
        if (piece0 == null)
        {
            return;
        }
        readyMerges.Add(piece0);

        CheckSymmetryMerge(piece0, Vector2Int.left);
        CheckSymmetryMerge(piece0, Vector2Int.up);
    }

    private void CheckSymmetryMerge(PieceInfo piece0, Vector2Int offset)
    {
        int preCount = readyMerges.Count;
        CheckDirectionMerge(piece0, offset);
        CheckDirectionMerge(piece0, -offset);
        int addCount = readyMerges.Count - preCount;
        if (addCount < 2)
        {
            readyMerges.RemoveRange(preCount, addCount);
        }
    }

    private void CheckDirectionMerge(PieceInfo piece0, Vector2Int offset)
    {
        Vector2Int offsetAdd = offset;
        PieceInfo next = GetMerge(piece0, offsetAdd);
        while (next != null)
        {
            readyMerges.Add(next);
            offsetAdd += offset;
            next = GetMerge(piece0, offsetAdd);
        }
    }

    private PieceInfo GetMerge(PieceInfo piece, Vector2Int offset)
    {
        PieceInfo pieceOffset = GetPiece(piece.index + offset);
        if (pieceOffset != null
            //&& piece.level <= 5 
            //&& pieceOffset.level == piece.level
            && pieceOffset.type == piece.type 
            )
        {
            return pieceOffset;
        }
        return null;
    }
}