using System;
using System.Collections.Generic;
using UnityEngine;

public class FrameMono : MonoBehaviour
{
    Dictionary<EventEnum, HashSet<EventHandler>> eventDic;

    protected virtual void Awake()
    {
        
    }

    protected void AutoListener(EventEnum eventKey, EventHandler callback)
    {
        eventDic ??= EventModel.Instance.UseDic();
        EventModel.AddListener(eventKey, callback, eventDic);
    }

    protected void SendEvent(EventEnum eventKey, object data = default)
    {
        EventModel.Send(eventKey, data);
    }

    protected virtual void OnDestroy()
    {
        if (eventDic != null)
        {
            EventModel.Instance.ReturnDic(eventDic);
        }
    }
}
