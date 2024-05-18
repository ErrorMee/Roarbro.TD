using System.Collections.Generic;
using UnityEngine;

public partial class PieceModel : Singleton<PieceModel>, IDestroy
{
    PieceInfo startPiece;
    public List<PieceInfo> upgradePieces = new List<PieceInfo>();
    public List<PieceInfo> removePieces = new List<PieceInfo>();

    public void ExcuteMerge()
    {
        startPiece = readyMerges[0];
        readyMerges.Sort(SortMerges);

        upgradePieces.Clear();
        removePieces.Clear();

        int bonus = Mathf.Max(0, readyMerges.Count - 3);
        int reward = bonus + combo;
        if (reward > 0)
        {
            WindowModel.Msg(LanguageModel.Get(10032) + " + " + reward);
        }

        int leftLevel = reward;
        for (int i = 0; i < readyMerges.Count; i++)
        {
            PieceInfo pieceInfo = readyMerges[i];
            leftLevel += pieceInfo.level;
        }

        for (int i = 0; i < readyMerges.Count; i++)
        {
            PieceInfo pieceInfo = readyMerges[i];
            if (leftLevel > 0)
            {
                if (leftLevel <= PieceMaxLV)
                {
                    pieceInfo.level = leftLevel;
                    upgradePieces.Add(pieceInfo);
                }
                else 
                {
                    pieceInfo.level = PieceMaxLV;
                    upgradePieces.Add(pieceInfo);
                }
                leftLevel -= PieceMaxLV;
            }
            else
            {
                pieceInfo.level = 1;
                removePieces.Add(pieceInfo);
            }
        }

        combo++;
    }

    private int SortMerges(PieceInfo a, PieceInfo b)
    {
        return b.GetMergePriority(startPiece) - a.GetMergePriority(startPiece);
    }
}