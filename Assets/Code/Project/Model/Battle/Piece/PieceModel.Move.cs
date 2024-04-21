using System.Collections.Generic;
using UnityEngine;

public partial class PieceModel : Singleton<PieceModel>, IDestroy
{
    private List<Vector2Int> dragIndexs = new List<Vector2Int>();

    public void DragPiece(PieceInfo fromInfo, Vector2Int toIndex)
    {
        if (fromInfo.index.x != toIndex.x && fromInfo.index.y != toIndex.y)
        {
            ChangeIndex(fromInfo, fromInfo.index);
        }
        else
        {
            if (fromInfo.index.x == toIndex.x && fromInfo.index.y == toIndex.y)
            {
                ChangeIndex(fromInfo, fromInfo.index);
            }
            else
            {
                if (toIndex.x >= 0 && toIndex.x < pieceInfos.GetLength(0) &&
                    toIndex.y >= 0 && toIndex.y < pieceInfos.GetLength(1))
                {
                    Vector2Int fromIndex = fromInfo.index;
                    PieceInfo toInfo = pieceInfos[toIndex.x, toIndex.y];
                    ChangeIndex(toInfo, fromIndex);
                    ChangeIndex(fromInfo, toIndex);

                    dragIndexs.Add(fromIndex);
                    dragIndexs.Add(toIndex);
                }
                else
                {
                    ChangeIndex(fromInfo, fromInfo.index);
                }
            }
        }
    }

    public void ChangeIndex(PieceInfo piece, Vector2Int index)
    {
        piece.index = index;
        pieceInfos[piece.index.x, piece.index.y] = piece;
        EventModel.Send(EventEnum.MovePiece, piece);
    }
}