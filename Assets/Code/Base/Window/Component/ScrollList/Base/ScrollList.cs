using EasingCore;
using FancyScrollView;
using System;
using UnityEngine;

[RequireComponent(typeof(Scroller))]
[DisallowMultipleComponent]
public class ScrollList<TInfo, TCell> : FancyGridView<TInfo, ScrollContext> 
    where TCell : FancyGridViewCell<TInfo, ScrollContext>
{
    [SerializeField] TCell cellPrefab = default;

    protected override bool Scrollable
    {//MaxScrollPosition > 0f
        get { return true; }
    }

    protected override void SetupCellTemplate()
    {
        cellPrefab.gameObject.SetActive(false);
    }

    protected void Setup<TGroup>() where TGroup : FancyCell<TInfo[], ScrollContext>
    {
        Setup<TGroup>(cellPrefab);
    }

    public float PaddingTop
    {
        get => paddingHead;
        set
        {
            paddingHead = value;
            Relayout();
        }
    }

    public float PaddingBottom
    {
        get => paddingTail;
        set
        {
            paddingTail = value;
            Relayout();
        }
    }

    public float SpacingY
    {
        get => spacing;
        set
        {
            spacing = value;
            Relayout();
        }
    }

    public float SpacingX
    {
        get => startAxisSpacing;
        set
        {
            startAxisSpacing = value;
            Relayout();
        }
    }

    public void SelectCell(int index, bool check = true)
    {
        if (Context.SelectedIndex == index && check)
        {
            return;
        }

        Context.SelectedIndex = index;
        Refresh();
    }

    public int SelectedIndex
    {
        get { return Context.SelectedIndex; }
    }

    public void OnSelected(Action<int> callback)
    {
        Context.OnCellClicked = callback;
    }

    public void ScrollTo(int index, float duration = 0.3f, Ease easing = Ease.Linear, Alignment alignment = Alignment.Middle)
    {
        SelectCell(index);
        ScrollTo(index, duration, easing, GetAlignment(alignment));
    }

    public void JumpTo(int index, Alignment alignment = Alignment.Middle)
    {
        SelectCell(index);
        JumpTo(index, GetAlignment(alignment));
    }

    float GetAlignment(Alignment alignment)
    {
        switch (alignment)
        {
            case Alignment.Upper: return 0.0f;
            case Alignment.Middle: return 0.5f;
            case Alignment.Lower: return 1.0f;
            default: return GetAlignment(Alignment.Middle);
        }
    }
}