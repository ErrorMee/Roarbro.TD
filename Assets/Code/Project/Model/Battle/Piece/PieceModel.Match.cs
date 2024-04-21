using System.Collections.Generic;
using UnityEngine;

public partial class PieceModel : Singleton<PieceModel>, IDestroy
{
    List<PieceInfo> matchs = new List<PieceInfo>();

    public void HandleMatchs(Vector2Int index)
    {
        matchs.Clear();
        PieceInfo piece0 = GetPiece(index);
        if (piece0 == null)
        {
            return;
        }
        matchs.Add(piece0);

        SymmetryMatch(piece0, Vector2Int.left);
        SymmetryMatch(piece0, Vector2Int.up);

        if (matchs.Count > 1)
        {
            Debug.LogError("matchs.Count: " + matchs.Count);
            for (int i = 0; i < matchs.Count; i++)
            {
                PieceInfo match = matchs[i];
                Debug.LogError(match.index + " : " + match.type);
            }
        }
    }

    private void SymmetryMatch(PieceInfo piece0, Vector2Int offset)
    {
        int preCount = matchs.Count;
        DirectionMatch(piece0, offset);
        DirectionMatch(piece0, -offset);
        int addCount = matchs.Count - preCount;
        if (addCount < 2)
        {
            matchs.RemoveRange(preCount, addCount);
        }
    }

    private void DirectionMatch(PieceInfo piece0, Vector2Int offset)
    {
        Vector2Int offsetAdd = offset;
        PieceInfo next = GetMatch(piece0, offsetAdd);
        while (next != null)
        {
            matchs.Add(next);
            offsetAdd += offset;
            next = GetMatch(piece0, offsetAdd);
        }
    }

    private PieceInfo GetMatch(PieceInfo piece, Vector2Int offset)
    {
        PieceInfo pieceOffset = GetPiece(piece.index + offset);
        if (pieceOffset != null && pieceOffset.type == piece.type)
        {
            return pieceOffset;
        }
        return null;
    }
}