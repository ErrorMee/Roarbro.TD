using System;

[Serializable]
public class FloatGrowth
{
    public float basic;
    public float growth;

    public float GetValue(uint level)
    {
        return basic + growth * (level - 1);
    }
}