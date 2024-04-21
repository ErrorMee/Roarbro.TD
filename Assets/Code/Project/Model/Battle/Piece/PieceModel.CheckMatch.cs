using System.Collections.Generic;
using UnityEngine;

public partial class PieceModel : Singleton<PieceModel>, IDestroy
{
    public List<PieceInfo> readyMatchs = new List<PieceInfo>();

    public void CheckMatchs()
    {
        for (int y = GridUtil.YMaxIndex; y >=0; y--)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                CheckMatchs(new Vector2Int(x, y));

                if (readyMatchs.Count > 2)
                {
                    return;
                }
            }
        }
    }

    public void CheckMatchs(Vector2Int index)
    {
        readyMatchs.Clear();
        PieceInfo piece0 = GetPiece(index);
        if (piece0 == null)
        {
            return;
        }
        readyMatchs.Add(piece0);

        CheckSymmetryMatch(piece0, Vector2Int.left);
        CheckSymmetryMatch(piece0, Vector2Int.up);
    }

    private void CheckSymmetryMatch(PieceInfo piece0, Vector2Int offset)
    {
        int preCount = readyMatchs.Count;
        CheckDirectionMatch(piece0, offset);
        CheckDirectionMatch(piece0, -offset);
        int addCount = readyMatchs.Count - preCount;
        if (addCount < 2)
        {
            readyMatchs.RemoveRange(preCount, addCount);
        }
    }

    private void CheckDirectionMatch(PieceInfo piece0, Vector2Int offset)
    {
        Vector2Int offsetAdd = offset;
        PieceInfo next = GetMatch(piece0, offsetAdd);
        while (next != null)
        {
            readyMatchs.Add(next);
            offsetAdd += offset;
            next = GetMatch(piece0, offsetAdd);
        }
    }

    private PieceInfo GetMatch(PieceInfo piece, Vector2Int offset)
    {
        PieceInfo pieceOffset = GetPiece(piece.index + offset);
        if (pieceOffset != null && pieceOffset.type == piece.type && pieceOffset.level == piece.level)
        {
            return pieceOffset;
        }
        return null;
    }
}