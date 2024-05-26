using System.Collections.Generic;
using UnityEngine;

public partial class BallModel : Singleton<BallModel>, IDestroy
{
    public void Fill()
    {
        for (int x = 0; x < GridUtil.XCount; x++)
        {
            for (int y = GridUtil.YMaxIndex; y >= 0;)
            {
                BallInfo crtBall = infos[x, y];
                if (crtBall.RemoveMark == true)
                {
                    int moveStep = 1;
                    for (int ym = y - 1; ym >= 0; ym--)
                    {
                        BallInfo moveBall = infos[x, ym];
                        if (moveBall.level > 1)
                        {
                            moveStep++;
                        }
                        else
                        {
                            MoveIndex(moveBall, new Vector2Int(x, ym + moveStep));
                            moveStep = 1;
                        }
                    }

                    int randomID = Random.Range(0, 6);
                    crtBall.config = BallConfigs.Instance.GetConfigByID(randomID);
                    crtBall.RemoveMark = false;
                    MoveIndex(crtBall, new Vector2Int(x, moveStep - 1));
                }
                else
                {
                    y--;
                }
            }
        }
    }
}