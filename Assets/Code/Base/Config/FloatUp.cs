using System;

[Serializable]
public class FloatUp
{
    public float basic;
    public float percent;

    public float GetValue(uint level)
    {
        return basic * percent * (level - 1);
    }
}