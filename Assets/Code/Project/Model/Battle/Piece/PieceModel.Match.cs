using System.Collections.Generic;
using UnityEngine;

public partial class PieceModel : Singleton<PieceModel>, IDestroy
{
    public List<PieceInfo> upgradePieces = new List<PieceInfo>();
    public List<PieceInfo> removePieces = new List<PieceInfo>();

    public void ExcuteMatch()
    {
        upgradePieces.Clear();

        int upgradeCount = readyMatchs.Count - 2;

        for (int i = 0; i < readyMatchs.Count; i++)
        {
            PieceInfo pieceInfo = readyMatchs[i];
            if (i < upgradeCount)
            {
                pieceInfo.level += 1;
                upgradePieces.Add(pieceInfo);
            }
            else
            {
                removePieces.Add(pieceInfo);
            }
        }
    }

}