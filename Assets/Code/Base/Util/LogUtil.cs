using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LogUtil
{
	static ILogger debug = Debug.unityLogger;

	static public void InitLogger()
	{
		debug = Debug.unityLogger;
		debug.filterLogType = LogType.Log;
		debug.logEnabled = true;
	}

	static public void Log(string str, Color color)
    {
        debug.Log(ColorStringUtil.ColorString(color, str));
    }

    static public void Log(string str, string str2, Color color)
    {
        debug.Log(ColorStringUtil.ColorString(color, str + str2));
    }
}
