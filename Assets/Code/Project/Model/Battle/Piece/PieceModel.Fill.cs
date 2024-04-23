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
                if (crtPiece.RremoveMark == true)
                {
                    int moveStep = 1;
                    for (int ym = y - 1; ym >= 0; ym--)
                    {
                        PieceInfo movePiece = pieceInfos[x, ym];
                        if (movePiece.level > 1)
                        {
                            moveStep++;
                        }
                        else
                        {
                            ChangeIndex(movePiece, new Vector2Int(x, ym + moveStep));
                            moveStep = 1;
                        }
                    }

                    int randomID = Random.Range(0, 6);
                    crtPiece.type = randomID;
                    crtPiece.RremoveMark = false;
                    ChangeIndex(crtPiece, new Vector2Int(x, moveStep - 1));
                }
                else
                {
                    y--;
                }
            }
        }
    }
}