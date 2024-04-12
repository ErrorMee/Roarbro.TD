public class TimerInfo
{
    float initTime;
    float leftTime;
    public float LeftTime
    {
        set
        {
            leftTime = initTime = value;
        }
        get
        {
            return leftTime;
        }
    }

    public bool Loop { set; get; } = false;

    public bool Pause { set; get; } = false;

    public bool Finished { set; get; } = false;

    public TimerCall Call { set; get; }

    public TimerInfo(){}

    public void Init(float interval, TimerCall call, bool loop = false)
    {
        LeftTime = interval;
        Loop = loop;
        Pause = false;
        Finished = false;
        Call = call;
    }

    public void Update(float cost)
    {
        if (Pause)
        {
            return;
        }
        
        leftTime -= cost;

        if (leftTime <= 0)
        {
            if (Loop)
            {
                leftTime += initTime;
            }
            else
            {
                Finished = true;
            }
            Call.Invoke();
        }
    }
}
