using System.Collections.Generic;
using UnityEngine;

public partial class BallModel : Singleton<BallModel>, IDestroy
{
    BallInfo startBall;
    public List<BallInfo> upgradeBalls = new List<BallInfo>();
    public List<BallInfo> removeBalls = new List<BallInfo>();

    public void ExcuteMerge()
    {
        startBall = readyMerges[0];
        readyMerges.Sort(SortMerges);

        upgradeBalls.Clear();
        removeBalls.Clear();

        int bonus = Mathf.Max(0, readyMerges.Count - 3);
        int reward = bonus + combo;
        if (reward > 0)
        {
            WindowModel.Msg(LanguageModel.Get(10032) + " + " + reward);
        }

        int leftLevel = reward;
        for (int i = 0; i < readyMerges.Count; i++)
        {
            BallInfo ballInfo = readyMerges[i];
            leftLevel += ballInfo.level;
        }

        for (int i = 0; i < readyMerges.Count; i++)
        {
            BallInfo ballInfo = readyMerges[i];
            if (leftLevel > 0)
            {
                if (leftLevel <= BallMaxLV)
                {
                    ballInfo.level = leftLevel;
                    if (ballInfo.config.id == 0)
                    {
                        removeBalls.Add(ballInfo);
                    }
                    else
                    {
                        upgradeBalls.Add(ballInfo);
                    }
                }
                else 
                {
                    ballInfo.level = BallMaxLV;
                    upgradeBalls.Add(ballInfo);
                }
                leftLevel -= BallMaxLV;
            }
            else
            {
                ballInfo.level = 1;
                removeBalls.Add(ballInfo);
            }
        }

        combo++;
    }

    private int SortMerges(BallInfo a, BallInfo b)
    {
        return b.GetMergePriority(startBall) - a.GetMergePriority(startBall);
    }
}