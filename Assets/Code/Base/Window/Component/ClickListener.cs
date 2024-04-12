using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.EventSystems.ExecuteEvents;

public class ClickListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, 
    IPointerEnterHandler, IPointerExitHandler
{ 
    public delegate void VoidDelegate();
    public delegate void PointerEventDelegate(PointerEventData eventData);

    public VoidDelegate onClick;
    public PointerEventDelegate onEventClick;
    public VoidDelegate onDown;
    public PointerEventDelegate onEventDown;
    public VoidDelegate onUp;
    public PointerEventDelegate onEventUp;
    public VoidDelegate onEnter;
    public PointerEventDelegate onEventEnter;
    public VoidDelegate onExit;
    public PointerEventDelegate onEventExit;

    /// <summary>
    /// 事件冒泡
    /// </summary>
    [ReadOnlyProperty]
    public bool bubbleEvent = false;

    public string clickAudio = "Click";

    static public ClickListener Add(UIBehaviour behavier, bool eventBubble = false)
    {
        return Add(behavier.transform, eventBubble);
    }

    static public ClickListener Add(GameObject gameObject, bool eventBubble = false)
    {
        return Add(gameObject.transform, eventBubble);
    }

    static public ClickListener Add(Transform transform, bool eventBubble = false)
    {
        if (!transform.TryGetComponent<ClickListener>(out var listener))
            listener = transform.gameObject.AddComponent<ClickListener>();

        listener.bubbleEvent = eventBubble;
        return listener;
    }

    static public bool Remove(Transform transform)
    {
        if (!transform.TryGetComponent<ClickListener>(out var listener))
        {
            return false;
        }
        else
        {
            Destroy(listener);
            return true;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bool hasCall = false;
        if (onClick != null)
        {
            onClick.Invoke(); hasCall = true;
        }

        if (onEventClick != null)
        {
            onEventClick.Invoke(eventData); hasCall = true;
        }

        if (hasCall)
        {
            AudioModel.PlaySound(clickAudio);
        }
        
        BubbleEvent(eventData, eventData.pointerEnter, pointerClickHandler);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onDown?.Invoke();
        onEventDown?.Invoke(eventData);
        BubbleEvent(eventData, eventData.pointerEnter, pointerDownHandler);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onUp?.Invoke();
        onEventUp?.Invoke(eventData);
        BubbleEvent(eventData, eventData.pointerEnter, pointerUpHandler);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onEnter?.Invoke();
        onEventEnter?.Invoke(eventData);
        BubbleEvent(eventData, eventData.pointerEnter, pointerUpHandler);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onExit?.Invoke();
        onEventExit?.Invoke(eventData);
        BubbleEvent(eventData, eventData.pointerEnter, pointerUpHandler);
    }

    private void BubbleEvent<T>(PointerEventData eventData, GameObject target, EventFunction<T> pointerEventHandler) 
        where T : IEventSystemHandler
    {
        if (bubbleEvent)
        {
            Transform parent = target.transform.parent;
            if (parent)
            {
                Graphic parentGraphic = parent.GetComponent<Graphic>();
                if (parentGraphic && parentGraphic.raycastTarget)
                {
                    Execute(parentGraphic.gameObject, eventData, pointerEventHandler);
                }
            }
        }
    }
}