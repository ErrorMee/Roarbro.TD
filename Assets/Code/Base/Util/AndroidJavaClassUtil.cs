using System;
using System.Collections.Generic;
using UnityEngine;

public static class AndroidJavaClassUtil
{
    public static AndroidJavaClass unityPlayerClass;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaClass service;

    public static void Init()
    {
        unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        service = new AndroidJavaClass("android.app.Service");
    }

}