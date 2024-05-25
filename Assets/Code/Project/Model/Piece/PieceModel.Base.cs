using UnityEngine;

public partial class PieceModel : Singleton<PieceModel>, IDestroy
{
    public PieceInfo[,] infos;

    public static int PieceMaxLV = 60;

    int combo = 0;

    public bool fighting = false;

    public PieceModel Init(BattleInfo battleInfo)
    {
        MaxStep = battleInfo.config.steps;

        infos = new PieceInfo[GridUtil.XCount, GridUtil.YCount];
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                PieceInfo info = new PieceInfo();

                infos[x, y] = info;
                int randomID = UnityEngine.Random.Range(0, 6);
                info.config = PieceConfigs.Instance.GetConfigByID(randomID);
                info.index = new Vector2Int(x, y);
            }
        }
        return Instance;
    }

    public PieceInfo GetPiece(Vector2Int index)
    {
        if (index.x >= 0 && index.x < infos.GetLength(0) &&
                    index.y >= 0 && index.y < infos.GetLength(1))
        {
            return infos[index.x, index.y];
        }
        else
        {
            return null;
        }
    }
}