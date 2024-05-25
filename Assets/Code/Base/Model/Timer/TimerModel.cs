using System.Collections.Generic;
using UnityEngine;

public delegate void TimerCall();

public class TimerModel : Singleton<TimerModel>, IDestroy
{
    private readonly Dictionary<TimerCall, TimerInfo> timerInfos = new();

    private readonly List<TimerCall> finishedTimers = new();

    public static void Init()
    {
        Time.fixedDeltaTime = 1f / 50.0f;
        Time.timeScale = 1;
    }

    public static TimerInfo Add(TimerCall timerCall, float delay, bool loop = false)
    {
        if (!Instance.timerInfos.TryGetValue(timerCall, out TimerInfo timerInfo))
        {
            timerInfo = SharedPool<TimerInfo>.Get();
            Instance.timerInfos.Add(timerCall, timerInfo);
        }
        timerInfo.Init(delay, timerCall, loop);
        return timerInfo;
    }

    public static void Remove(TimerCall timerCall)
    {
        if (Instance.timerInfos.ContainsKey(timerCall)) 
        {
            SharedPool<TimerInfo>.Cache(Instance.timerInfos[timerCall]);
            Instance.timerInfos.Remove(timerCall);
        }
    }

    public void FixedUpdate()
    {
        foreach (var item in timerInfos)
        {
            item.Value.Update(Time.fixedDeltaTime);
            if (item.Value.Finished)
            {
                finishedTimers.Add(item.Key);
            }
        }

        foreach (var item in finishedTimers)
        {
            SharedPool<TimerInfo>.Cache(timerInfos[item]);
            timerInfos.Remove(item);
        }
        finishedTimers.Clear();
    }
}