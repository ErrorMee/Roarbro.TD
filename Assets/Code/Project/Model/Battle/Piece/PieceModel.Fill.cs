using System.Collections.Generic;
using UnityEngine;

public partial class PieceModel : Singleton<PieceModel>, IDestroy
{
    public void Fill()
    {
        for (int x = 0; x < GridUtil.XCount; x++)
        {
            for (int y = GridUtil.YMaxIndex; y >= 0;)
            {
                PieceInfo crtPiece = pieceInfos[x, y];
                if (crtPiece.DeleteMark == true)
                {
                    for (int ym = y - 1; ym >= 0; ym--)
                    {
                        PieceInfo movePiece = pieceInfos[x, ym];
                        ChangeIndex(movePiece, new Vector2Int(x, ym + 1));
                    }

                    int randomID = UnityEngine.Random.Range(0, 6);
                    crtPiece.type = randomID;
                    crtPiece.DeleteMark = false;
                    ChangeIndex(crtPiece, new Vector2Int(x, 0));
                }
                else
                {
                    y--;
                }
            }
        }
    }
}