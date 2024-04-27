using System.Collections.Generic;
using UnityEngine;

public partial class PieceModel : Singleton<PieceModel>, IDestroy
{
    private List<Vector2Int> dragIndexs = new List<Vector2Int>();

    public void DragPiece(PieceInfo fromInfo, Vector2Int toIndex)
    {
        combo = 0;
        if (fromInfo.index.x != toIndex.x && fromInfo.index.y != toIndex.y)
        {
            MoveIndex(fromInfo, fromInfo.index);
        }
        else
        {
            if (fromInfo.index.x == toIndex.x && fromInfo.index.y == toIndex.y)
            {
                MoveIndex(fromInfo, fromInfo.index);
            }
            else
            {
                if (toIndex.x >= 0 && toIndex.x < pieceInfos.GetLength(0) &&
                    toIndex.y >= 0 && toIndex.y < pieceInfos.GetLength(1))
                {
                    Vector2Int fromIndex = fromInfo.index;
                    PieceInfo toInfo = pieceInfos[toIndex.x, toIndex.y];
                    MoveIndex(toInfo, fromIndex);
                    MoveIndex(fromInfo, toIndex);

                    dragIndexs.Add(fromIndex);
                    dragIndexs.Add(toIndex);
                }
                else
                {
                    MoveIndex(fromInfo, fromInfo.index);
                }
            }
        }
    }

    public void MoveIndex(PieceInfo piece, Vector2Int index)
    {
        piece.index = index;
        pieceInfos[piece.index.x, piece.index.y] = piece;
        EventModel.Send(EventEnum.MovePiece, piece);
    }
}