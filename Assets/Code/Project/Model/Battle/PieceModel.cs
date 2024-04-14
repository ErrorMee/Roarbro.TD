
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
                pieceInfo.posx = x;
                pieceInfo.posy = y;
                
            }
        }
        return Instance;
    }

    public void DragDown(PieceInfo fromInfo, Vector2Int toIndex)
    {
        if (fromInfo.posx != toIndex.x && fromInfo.posy != toIndex.y)
        {
            MovePiece(fromInfo, fromInfo.posx, fromInfo.posy);
        }
        else
        {
            if (fromInfo.posx == toIndex.x && fromInfo.posy == toIndex.y)
            {
                MovePiece(fromInfo, fromInfo.posx, fromInfo.posy);
            }
            else
            {
                if (toIndex.x >= 0 && toIndex.x < pieceInfos.GetLength(0) &&
                    toIndex.y >= 0 && toIndex.y < pieceInfos.GetLength(1))
                {
                    PieceInfo toInfo = pieceInfos[toIndex.x, toIndex.y];
                    MovePiece(toInfo, fromInfo.posx, fromInfo.posy);
                    MovePiece(fromInfo, toIndex.x, toIndex.y);
                }
                else
                {
                    MovePiece(fromInfo, fromInfo.posx, fromInfo.posy);
                }
            }
        }
    }

    private void MovePiece(PieceInfo piece, int posx, int posy)
    {
        Debug.Log("MovePiece " + posx + " - " + posy);
        piece.posx = posx; piece.posy = posy;
        pieceInfos[piece.posx, piece.posy] = piece;
        EventModel.Send(EventEnum.MovePiece, piece);
    }
}