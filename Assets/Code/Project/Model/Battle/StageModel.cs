using System;
using System.Collections.Generic;
using UnityEngine;

public partial class StageModel : Singleton<StageModel>, IDestroy
{
    public BattleInfo[] battles;

    public int SelectIndex
    {
        set; get;
    } = -1;

    public int UnLockIndex
    {
        set; get;
    } = -1;

    public void Init()
    {
        battles = new BattleInfo[BattleConfigs.All.Length];
        for (int i = 0; i < BattleConfigs.All.Length; i++)
        {
            BattleConfig config = BattleConfigs.All[i];
            battles[i] = new BattleInfo(config);
        }
        SelectIndex = UnLockIndex = ArchiveModel.GetInt(ArchiveEnum.StageUnlockIdx, 0, true);
    }

    public bool IsUnLock(int index)
    {
        return index <= UnLockIndex;
    }

    public bool Complete()
    {
        if (SelectIndex == UnLockIndex)
        {
            UnLockIndex++;
            ArchiveModel.SetInt(ArchiveEnum.StageUnlockIdx, UnLockIndex);
            return true;
        }
        return false;
    }
}