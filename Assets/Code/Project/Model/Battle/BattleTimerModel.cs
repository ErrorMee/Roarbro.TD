using System;
using System.Collections.Generic;

public enum TimerEnum : ushort
{
    /// <summary>
    /// 渲染运算
    /// </summary>
    Render,
    /// <summary>
    /// 逻辑运算
    /// </summary>
    Logic,
    /// <summary>
    /// 滴答，一秒钟运算一次
    /// </summary>
    Tick,
}

public class BattleTimerModel : Singleton<BattleTimerModel>, IDestroy
{
    public const float renderInterval = 1f / 45.0f;
    public const float logicInterval = 1f / 50.0f;

    readonly Dictionary<TimerEnum, float> intervalDic = new()
    {
        { TimerEnum.Render, renderInterval},
        { TimerEnum.Logic, logicInterval},
        { TimerEnum.Tick, 1f }
    };
    public TimerInfo logicTimer, renderTimer, tickTimer;
    readonly Dictionary<TimerEnum, HashSet<Action>> actionDic = new();
    readonly Dictionary<TimerEnum, List<Action>> actionModCache = new();
    readonly Dictionary<TimerEnum, List<bool>> actionModOpCache = new();
    /// <summary>
    /// 持续时间
    /// </summary>
    public float Duration { private set;  get; } = 0;

    /// <summary>
    /// 倒计时
    /// </summary>
    public short CountDown
    {
        get { return countDown; }
        private set
        {
            if (value < 0)
            {
                value = 0;
            }
            if (countDown != value)
            {
                countDown = value;
            }
        }
    } short countDown;

    public short TickCount {
        get
        {
            return tickCount;
        }
        private set
        {
            tickCount = value;
        }
    }
    short tickCount = -1;

    public bool PauseTick { set; get; } = false;

    public BattleTimerModel Init()
    {
        Duration = 0;
        countDown = short.MaxValue;
        TickCount = -1;


        actionDic.Clear();
        actionDic.Add(TimerEnum.Render, new());
        actionDic.Add(TimerEnum.Logic, new());
        actionDic.Add(TimerEnum.Tick, new());
        actionModCache.Clear();
        actionModCache.Add(TimerEnum.Render, new());
        actionModCache.Add(TimerEnum.Logic, new());
        actionModCache.Add(TimerEnum.Tick, new());
        actionModOpCache.Clear();
        actionModOpCache.Add(TimerEnum.Render, new());
        actionModOpCache.Add(TimerEnum.Logic, new());
        actionModOpCache.Add(TimerEnum.Tick, new());

        logicTimer = TimerModel.Add(OnLogic, intervalDic[TimerEnum.Logic], true);
        renderTimer = TimerModel.Add(OnRender, intervalDic[TimerEnum.Render], true);
        tickTimer = TimerModel.Add(OnTick, intervalDic[TimerEnum.Tick], true);

        return Instance;
    }

    private void OnLogic()
    {
        Duration += logicInterval;
        Traverse(TimerEnum.Logic);
    }

    private void OnRender()
    {
        Traverse(TimerEnum.Render);
    }

    private void OnTick()
    {
        if (PauseTick)
        {
            return;
        }
        CountDown--;
        TickCount++;
        Traverse(TimerEnum.Tick);
    }

    /// <summary>
    /// 遍历
    /// </summary>
    /// <param name="timerEnum"></param>
    private void Traverse(TimerEnum timerEnum)
    {
        HashSet<Action> actions = actionDic[timerEnum];

        foreach (Action action in actions)
        {
            action.Invoke();
        }

        List<Action> actionsMod = Instance.actionModCache[timerEnum];
        List<bool> actionsModOp = Instance.actionModOpCache[timerEnum];

        for (int i = 0; i < actionsMod.Count; i++)
        {
            Action action = actionsMod[i];
            bool actionOp = actionsModOp[i];
            if (actionOp)
            {
                actions.Add(action);
            }
            else
            {
                actions.Remove(action);
            }
        }
        actionsMod.Clear();
        actionsModOp.Clear();
    }

    public static void AddImmediately(Action action, TimerEnum timerEnum)
    {
        HashSet<Action> actions = Instance.actionDic[timerEnum];
        actions.Add(action);
    }

    public static void Add(Action action, TimerEnum timerEnum)
    {
        List<Action> actionsMod = Instance.actionModCache[timerEnum];
        List<bool> actionsModOp = Instance.actionModOpCache[timerEnum];
        actionsMod.Add(action); actionsModOp.Add(true);
    }

    public static void Remove(Action action, TimerEnum timerEnum)
    {
        List<Action> actionsMod = Instance.actionModCache[timerEnum];
        List<bool> actionsModOp = Instance.actionModOpCache[timerEnum];
        actionsMod.Add(action); actionsModOp.Add(false);
    }

    public static void PauseAll()
    {
        Instance.logicTimer.Pause = Instance.renderTimer.Pause = Instance.tickTimer.Pause = true;
    }

    public static void Countinue()
    {
        Instance.logicTimer.Pause = Instance.renderTimer.Pause = Instance.tickTimer.Pause = false;
    }

    public override void Destroy()
    {
        TimerModel.Remove(OnRender);
        TimerModel.Remove(OnLogic);
        TimerModel.Remove(OnTick);
        base.Destroy();
    }
}