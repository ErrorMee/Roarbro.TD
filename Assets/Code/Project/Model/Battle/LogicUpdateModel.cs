using System;
using System.Collections.Generic;

public class LogicUpdateModel : Singleton<LogicUpdateModel>, IDestroy
{
    const float interval = 1f / 50.0f;

    public TimerInfo timer;
    readonly HashSet<Action> actionDic = new();
    readonly List<Action> actionCache = new();
    readonly List<bool> operateCache = new();

    public LogicUpdateModel Init()
    {
        actionDic.Clear();
        actionCache.Clear();
        operateCache.Clear();
        timer = TimerModel.Add(OnLogic, interval, true);
        return Instance;
    }

    private void OnLogic()
    {
        foreach (Action action in actionDic)
        {
            action.Invoke();
        }

        for (int i = 0; i < Instance.actionCache.Count; i++)
        {
            Action action = Instance.actionCache[i];
            bool actionOp = Instance.operateCache[i];
            if (actionOp)
            {
                actionDic.Add(action);
            }
            else
            {
                actionDic.Remove(action);
            }
        }
        Instance.actionCache.Clear();
        Instance.operateCache.Clear();
    }

    public static void Add(Action action)
    {
        Instance.actionCache.Add(action); 
        Instance.operateCache.Add(true);
    }

    public static void Remove(Action action)
    {
        Instance.actionCache.Add(action); 
        Instance.operateCache.Add(false);
    }

    public override void Destroy()
    {
        TimerModel.Remove(OnLogic);
        base.Destroy();
    }
}