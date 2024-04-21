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
}