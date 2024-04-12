using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragListener : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public delegate void OnDragEventHandler(PointerEventData eventData);
    public OnDragEventHandler onEventBeginDrag;
    public OnDragEventHandler onEventDrag;
    public Action<Vector2> onDeltaPos;
    public OnDragEventHandler onEventEndDrag;

    public Vector2 lastPos;

    static public DragListener Add(Transform transform)
    {
        if (!transform.TryGetComponent<DragListener>(out var listener))
            listener = transform.gameObject.AddComponent<DragListener>();
        return listener;
    }

    static public bool Remove(Transform transform)
    {
        if (!transform.TryGetComponent<DragListener>(out var listener))
        {
            return false;
        }
        else
        {
            Destroy(listener);
            return true;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        onEventBeginDrag?.Invoke(eventData);
        lastPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        onEventDrag?.Invoke(eventData);
        onDeltaPos?.Invoke(eventData.position - lastPos);
        lastPos = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        onEventEndDrag?.Invoke(eventData);
    }
}

