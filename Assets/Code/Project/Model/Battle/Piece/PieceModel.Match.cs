using System.Collections.Generic;

public partial class PieceModel : Singleton<PieceModel>, IDestroy
{
    PieceInfo startPiece;
    public PieceInfo upgradePiece;
    public List<PieceInfo> removePieces = new List<PieceInfo>();

    public void ExcuteMatch()
    {
        startPiece = readyMatchs[0];
        readyMatchs.Sort(SortMatchs);

        upgradePiece = readyMatchs[0];
        removePieces.Clear();

        int addLevel = 0;
        for (int i = 1; i < readyMatchs.Count; i++)
        {
            PieceInfo pieceInfo = readyMatchs[i];
            addLevel += pieceInfo.level;
            removePieces.Add(pieceInfo);
            pieceInfo.level = 1;
        }
        upgradePiece.level += addLevel;
    }

    private int SortMatchs(PieceInfo a, PieceInfo b)
    {
        return b.GetMatchPriority(startPiece) - a.GetMatchPriority(startPiece);
    }
}