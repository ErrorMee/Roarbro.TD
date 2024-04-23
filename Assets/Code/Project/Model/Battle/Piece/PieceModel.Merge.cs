using System.Collections.Generic;

public partial class PieceModel : Singleton<PieceModel>, IDestroy
{
    PieceInfo startPiece;
    public PieceInfo upgradePiece;
    public List<PieceInfo> removePieces = new List<PieceInfo>();

    public void ExcuteMerge()
    {
        startPiece = readyMerges[0];
        readyMerges.Sort(SortMerges);

        upgradePiece = readyMerges[0];
        removePieces.Clear();

        int addLevel = 0;
        for (int i = 1; i < readyMerges.Count; i++)
        {
            PieceInfo pieceInfo = readyMerges[i];
            addLevel += pieceInfo.level;
            removePieces.Add(pieceInfo);
            pieceInfo.level = 1;
        }
        upgradePiece.level += addLevel;
    }

    private int SortMerges(PieceInfo a, PieceInfo b)
    {
        return b.GetMergePriority(startPiece) - a.GetMergePriority(startPiece);
    }
}