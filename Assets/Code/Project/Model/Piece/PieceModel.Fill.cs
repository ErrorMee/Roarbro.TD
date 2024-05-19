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
                PieceInfo crtPiece = infos[x, y];
                if (crtPiece.RemoveMark == true)
                {
                    int moveStep = 1;
                    for (int ym = y - 1; ym >= 0; ym--)
                    {
                        PieceInfo movePiece = infos[x, ym];
                        if (movePiece.level > 1)
                        {
                            moveStep++;
                        }
                        else
                        {
                            MoveIndex(movePiece, new Vector2Int(x, ym + moveStep));
                            moveStep = 1;
                        }
                    }

                    int randomID = Random.Range(0, 6);
                    crtPiece.config = PieceConfigs.Instance.GetConfigByID(randomID);
                    crtPiece.RemoveMark = false;
                    MoveIndex(crtPiece, new Vector2Int(x, moveStep - 1));
                }
                else
                {
                    y--;
                }
            }
        }
    }
}