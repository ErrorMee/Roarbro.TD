using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowLayer : MonoBehaviour
{
    private readonly LinkedList<WindowBase> m_OrderWindows = new();

    public CanvasGroup canvasGroup;

    public void Init(WindowLayerEnum windowLayerEnum, int sortOrder)
    {
        name = windowLayerEnum.ToString();
        gameObject.GetOrAddComponent<GraphicRaycaster>();
        RectTransform rectTransform = gameObject.GetOrAddComponent<RectTransform>();
        
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = rectTransform.offsetMax = Vector2.zero;
        rectTransform.gameObject.layer = LayerMask.NameToLayer("UI");

        Canvas canvas = gameObject.GetOrAddComponent<Canvas>();
        canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.None;
        canvas.vertexColorAlwaysGammaSpace = false;
        canvas.overrideSorting = true;
        canvas.sortingOrder = sortOrder;

        canvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
    }

	/// <summary>
	/// 增加界面到最后
	/// </summary>
	/// <param name="window"></param>
	public void AddWindow(WindowBase window)
    {
        if (window.config.layer != WindowLayerEnum.Bot)
        {
            if (m_OrderWindows.Contains(window))
            {
                return;
            }
        }

        if (m_OrderWindows.Last != null)
        {
            if (m_OrderWindows.Last.Value != window)
            {
                m_OrderWindows.AddLast(window);
            }
        }
        else {
            m_OrderWindows.AddLast(window);
        }
    }

    /// <summary>
    /// 移除界面
    /// </summary>
    /// <param name="window"></param>
    /// <param name="lastOrAll">移除最近一个，还是所有</param>
    public void RemoveWindow(WindowBase window, bool lastOrAll = true)
    {
        LinkedListNode<WindowBase> crt = m_OrderWindows.Last;
        LinkedListNode<WindowBase> previous;
        LinkedListNode<WindowBase> previousPrevious;
        LinkedListNode<WindowBase> next;
        while (crt != null)
        {
            previous = crt.Previous;
            next = crt.Next;
            if (crt.Value == window)
            {
                m_OrderWindows.Remove(crt);
                if (previous != null && previous == next)
                {
                    previousPrevious = previous.Previous;
                    m_OrderWindows.Remove(previous);
                    previous = previousPrevious;
                }
                if (lastOrAll)
                {
                    return;
                }
            }
            crt = previous;
        }
    }

    /// <summary>
    /// 获取所有界面
    /// </summary>
    public LinkedList<WindowBase> GetWindows()
    {
        return m_OrderWindows;
    }

    /// <summary>
    /// 获取靠后界面
    /// </summary>
    /// <param name="toLastOffset">和最后的距离</param>
    public WindowBase GetLastWindow(int toLastOffset = 0)
    {
        LinkedListNode<WindowBase> crt = m_OrderWindows.Last;
        for (int i = 0; i < toLastOffset; i++)
        {
            if (crt == null)
            {
                return null;
            }
            crt = crt.Previous;
        }
        if (crt == null)
        {
            return null;
        }
        return crt.Value;
    }

    /// <summary>
    /// 查找界面
    /// </summary>
    public WindowBase GetWindow(int id)
    {
        LinkedListNode<WindowBase> crt = m_OrderWindows.Last;
        LinkedListNode<WindowBase> next;
        while (crt != null)
        {
            next = crt.Previous;
            if (crt.Value.config.id == id)
            {
                return crt.Value;
            }
            crt = next;
        }
        return null;
    }

    /// <summary>
    /// 界面在序列中的使用次数
    /// </summary>
    /// <returns></returns>
    public int GetWindowUseCount(int id)
    {
        int useCount = 0;
        LinkedListNode<WindowBase> crt = m_OrderWindows.Last;
        LinkedListNode<WindowBase> next;
        while (crt != null)
        {
            next = crt.Previous;
            if (crt.Value.config.id == id)
            {
                useCount ++;
            }
            crt = next;
        }
        return useCount;
    }
}