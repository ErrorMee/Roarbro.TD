using System.Collections.Generic;
using UnityEngine;

public partial class PieceModel : Singleton<PieceModel>, IDestroy
{
    public List<PieceInfo> upgradePieces = new List<PieceInfo>();
    public List<PieceInfo> removePieces = new List<PieceInfo>();

    public void ExcuteMatch()
    {
        upgradePieces.Clear();
        removePieces.Clear();

        //int upgradeCount = readyMatchs.Count - 2;
        //for (int i = 0; i < readyMatchs.Count; i++)
        //{
        //    PieceInfo pieceInfo = readyMatchs[i];
        //    if (i < upgradeCount)
        //    {
        //        pieceInfo.level += 1;
        //        upgradePieces.Add(pieceInfo);
        //    }
        //    else
        //    {
        //        removePieces.Add(pieceInfo);
        //    }
        //}

        PieceInfo pieceInfo0 = readyMatchs[0];
        int addLevel = 0;
        for (int i = 1; i < readyMatchs.Count; i++)
        {
            PieceInfo pieceInfo = readyMatchs[i];
            addLevel += pieceInfo.level;
            removePieces.Add(pieceInfo);
            pieceInfo.level = 1;
        }
        pieceInfo0.level += addLevel;
        upgradePieces.Add(pieceInfo0);
    }
}