using System.Collections.Generic;

public delegate void EventHandler(object data = null);

public partial class EventModel : Singleton<EventModel>, IDestroy
{
    readonly Dictionary<EventEnum, HashSet<EventHandler>> eventDic = new();

    readonly List<Dictionary<EventEnum, HashSet<EventHandler>>> managedEventDic = new();

    public static void RemoveListener(EventEnum eventKey, EventHandler callback)
    {
        Instance.OnRemoveListener(Instance.eventDic, eventKey, callback);
        for (int i = 0; i < Instance.managedEventDic.Count; i++)
        {
            Instance.OnRemoveListener(Instance.managedEventDic[i], eventKey, callback);
        }
    }

    public Dictionary<EventEnum, HashSet<EventHandler>> UseDic()
    {
        Dictionary<EventEnum, HashSet<EventHandler>> eventDic = SharedPool<Dictionary<EventEnum, HashSet<EventHandler>>>.Get();
        managedEventDic.Add(eventDic);
        return eventDic;
    }

    public void ReturnDic(Dictionary<EventEnum, HashSet<EventHandler>> eventDic)
    {
        foreach (HashSet<EventHandler> eventHandlers in eventDic.Values)
        {
            eventHandlers.Clear();
            SharedPool<HashSet<EventHandler>>.Cache(eventHandlers);
        }
        eventDic.Clear();
        managedEventDic.Remove(eventDic);
        SharedPool<Dictionary<EventEnum, HashSet<EventHandler>>>.Cache(eventDic);
    }

    private void OnRemoveListener(Dictionary<EventEnum, HashSet<EventHandler>> eventDic, EventEnum eventKey, EventHandler callback)
    {
        if (eventDic.TryGetValue(eventKey, out HashSet<EventHandler> callbacks))
        {
            callbacks.Remove(callback);
        }
    }

    private void OnSend(Dictionary<EventEnum, HashSet<EventHandler>> eventDic, EventEnum eventKey, object data)
    {
        if (eventDic.TryGetValue(eventKey, out HashSet<EventHandler> callbacks))
        {
            foreach (EventHandler eventCallback in callbacks)
            {
                eventCallback.Invoke(data);
            }
        }
    }

    public void ClearListenerByKey(EventEnum eventKey)
    {
        eventDic.Remove(eventKey);
        for (int i = 0; i < managedEventDic.Count; i++)
        {
            managedEventDic[i].Remove(eventKey);
        }
    }

    public void ClearAllListeners()
    {
        eventDic.Clear();
        for (int i = 0; i < managedEventDic.Count; i++)
        {
            managedEventDic[i].Clear();
        }
        managedEventDic.Clear();
    }
}