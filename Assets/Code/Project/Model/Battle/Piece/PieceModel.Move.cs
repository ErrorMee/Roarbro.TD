using UnityEngine;

public partial class PieceModel : Singleton<PieceModel>, IDestroy
{
    public void MovePiece(PieceInfo fromInfo, Vector2Int toIndex)
    {
        if (fromInfo.index.x != toIndex.x && fromInfo.index.y != toIndex.y)
        {
            OnMovePiece(fromInfo, fromInfo.index);
        }
        else
        {
            if (fromInfo.index.x == toIndex.x && fromInfo.index.y == toIndex.y)
            {
                OnMovePiece(fromInfo, fromInfo.index);
            }
            else
            {
                if (toIndex.x >= 0 && toIndex.x < pieceInfos.GetLength(0) &&
                    toIndex.y >= 0 && toIndex.y < pieceInfos.GetLength(1))
                {
                    Vector2Int fromIndex = fromInfo.index;
                    PieceInfo toInfo = pieceInfos[toIndex.x, toIndex.y];
                    OnMovePiece(toInfo, fromIndex);
                    OnMovePiece(fromInfo, toIndex);

                    CheckMatchs(fromIndex);
                    CheckMatchs(toIndex);
                }
                else
                {
                    OnMovePiece(fromInfo, fromInfo.index);
                }
            }
        }
    }

    public void OnMovePiece(PieceInfo piece, Vector2Int index)
    {
        piece.index = index;
        pieceInfos[piece.index.x, piece.index.y] = piece;
        EventModel.Send(EventEnum.MovePiece, piece);
    }
}