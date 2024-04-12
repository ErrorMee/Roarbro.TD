using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[DisallowMultipleComponent]
public class ThreadingUtil : MonoBehaviour
{
    private static ThreadingUtil _current;

    public static void Init()
    {
        if (_current == null)
        {
            GameObject g = new GameObject("ThreadingUtil");
            DontDestroyOnLoad(g);
            _current = g.AddComponent<ThreadingUtil>();
        }
    }

    List<Action> _actions = new List<Action>();
    public static void ActionOnMainThread(Action action)
    {
        lock (_current._actions)
        {
            _current._actions.Add(action);
        }
    }

    public static void ActionOnMutiThread(Action action)
    {
        Init();
        var t = new Thread(RunAction);
        t.Start(t);
    }

    private static void RunAction(object action)
    {
        ((Action)action)();
    }

    List<Action> _invokeActions = new List<Action>();
    private void Update()
    {
        lock (_actions)
        {
            _invokeActions.Clear();
            _invokeActions.AddRange(_actions);
            _actions.Clear();
        }
        foreach (var invokeAction in _invokeActions)
        {
            invokeAction?.Invoke();
        }
    }
}