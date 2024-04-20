using UnityEngine;

public partial class PieceModel : Singleton<PieceModel>, IDestroy
{
    public PieceInfo[,] pieceInfos;

    public PieceModel Init()
    {
        pieceInfos = new PieceInfo[GridUtil.XCount, GridUtil.YCount];
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                PieceInfo pieceInfo = new PieceInfo();

                pieceInfos[x, y] = pieceInfo;
                int randomID = UnityEngine.Random.Range(0, 6);
                pieceInfo.type = randomID;
                pieceInfo.level = 0;
                pieceInfo.index = new Vector2Int(x, y);
            }
        }
        return Instance;
    }

    public PieceInfo GetPiece(Vector2Int index)
    {
        if (index.x >= 0 && index.x < pieceInfos.GetLength(0) &&
                    index.y >= 0 && index.y < pieceInfos.GetLength(1))
        {
            return pieceInfos[index.x, index.y];
        }
        else
        {
            return null;
        }
    }

    private void MovePiece(PieceInfo piece, Vector2Int index)
    {
        piece.index = index;
        pieceInfos[piece.index.x, piece.index.y] = piece;
        EventModel.Send(EventEnum.MovePiece, piece);
    }

    public void DragDown(PieceInfo fromInfo, Vector2Int toIndex)
    {
        if (fromInfo.index.x != toIndex.x && fromInfo.index.y != toIndex.y)
        {
            MovePiece(fromInfo, fromInfo.index);
        }
        else
        {
            if (fromInfo.index.x == toIndex.x && fromInfo.index.y == toIndex.y)
            {
                MovePiece(fromInfo, fromInfo.index);
            }
            else
            {
                if (toIndex.x >= 0 && toIndex.x < pieceInfos.GetLength(0) &&
                    toIndex.y >= 0 && toIndex.y < pieceInfos.GetLength(1))
                {
                    Vector2Int fromIndex = fromInfo.index;
                    PieceInfo toInfo = pieceInfos[toIndex.x, toIndex.y];
                    MovePiece(toInfo, fromIndex);
                    MovePiece(fromInfo, toIndex);

                    HandleMatchs(fromIndex);
                    HandleMatchs(toIndex);
                }
                else
                {
                    MovePiece(fromInfo, fromInfo.index);
                }
            }
        }
    }
}