using System;
using System.Collections.Generic;
using UnityEngine;

public partial class EventModel
{
    public static void AddListener(EventEnum eventKey, EventHandler callback,
    Dictionary<EventEnum, HashSet<EventHandler>> eventDic = null)
    {
        eventDic ??= Instance.eventDic;
        if (eventDic.TryGetValue(eventKey, out HashSet<EventHandler> callbacks))
        {
            if (!callbacks.Contains(callback))
            {
                callbacks.Add(callback);
            }
        }
        else
        {
            callbacks = SharedPool<HashSet<EventHandler>>.Get();
            callbacks.Add(callback);
            eventDic.Add(eventKey, callbacks);
        }
    }

    public static void Send(EventEnum eventKey, object data = default)
    {
        Instance.OnSend(Instance.eventDic, eventKey, data);
        for (int i = 0; i < Instance.managedEventDic.Count; i++)
        {
            Instance.OnSend(Instance.managedEventDic[i], eventKey, data);
        }
    }
}