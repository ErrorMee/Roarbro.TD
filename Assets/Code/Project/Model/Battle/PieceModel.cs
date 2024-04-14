
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

    public void DragDown(PieceInfo fromInfo, Vector2Int to)
    {
        if (fromInfo.posx != to.x && fromInfo.posy != to.y)
        {
            EventModel.Send(EventEnum.ResetPieces);
        }
        else
        {
            if (fromInfo.posx == to.x && fromInfo.posy == to.y)
            {
                EventModel.Send(EventEnum.ResetPieces);
            }
            else
            {
                if (to.x < pieceInfos.GetLength(0) && to.y < pieceInfos.GetLength(1))
                {
                    PieceInfo infoTo = pieceInfos[to.x, to.y];

                    infoTo.posx = fromInfo.posx; infoTo.posy = fromInfo.posy;
                    pieceInfos[infoTo.posx, infoTo.posy] = infoTo;

                    fromInfo.posx = to.x; fromInfo.posy = to.y;
                    pieceInfos[to.x, to.y] = fromInfo;

                    EventModel.Send(EventEnum.MovePiece, infoTo);
                    EventModel.Send(EventEnum.MovePiece, fromInfo);
                    EventModel.Send(EventEnum.ResetPieces);
                }
                else
                {
                    EventModel.Send(EventEnum.ResetPieces);
                }
            }
        }
    }
}