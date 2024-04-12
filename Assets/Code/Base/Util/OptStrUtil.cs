
using UnityEngine;

public static class OptStrUtil
{
	public static void Init()
	{
        for (int i = 0; i < 100; i++)
        {
            string v = i.ToString();
            Strings0To999[i] = v;
            if (i < 10)
            {
                Strings00To99[i] = "0" + v;
            }
            else
            {
                Strings00To99[i] = v;
            }
            
            Strings0kTo999k[i] = v + "K";
            Strings0MTo999M[i] = v + "M";
        }

        for (int i = 100; i < 1000; i++)
		{
            string v = i.ToString();
            Strings0To999[i] = v;
			Strings0kTo999k[i] = v + "K";
            Strings0MTo999M[i] = v + "M";
        }
    }

    static readonly string[] Strings00To99 = new string[100];
    static readonly string[] Strings0To999 = new string[1000];
	static readonly string[] Strings0kTo999k = new string[1000];
    static readonly string[] Strings0MTo999M = new string[1000];

    public static string OptStr(this int value)
    {
        if (value > 999999)
        {
            return Strings0MTo999M[Mathf.FloorToInt(value / 1000000)];
        }
        if (value > 999)
        {
            return Strings0kTo999k[Mathf.FloorToInt(value / 1000)];
        }
        return Strings0To999[value];
    }

    public static string Opt00Str(this int value)
    {
        return Strings00To99[value];
    }
}
