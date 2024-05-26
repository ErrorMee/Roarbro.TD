using FancyScrollView;
using UnityEngine;
using System;
using System.Collections.Generic;
using EasingCore;

[RequireComponent(typeof(Scroller))]
[DisallowMultipleComponent]
public class ScrollFocus<TInfo, TCell> : FancyScrollView<TInfo, ScrollFocusContext>
    where TInfo : class where TCell : ScrollFocusCell<TInfo>
{
    Scroller scroller = default;
    [SerializeField] GameObject cellPrefab = default;

    protected Action<int> onSelectionChanged;

    protected override GameObject CellPrefab => cellPrefab;

    protected override void Initialize()
    {
        if (cellPrefab.transform.parent != null)
        {
            cellPrefab.SetActive(false);
        }
        base.Initialize();
        scroller = GetComponent<Scroller>();
        Context.OnCellClicked = SelectCell;

        scroller.OnValueChanged(UpdatePosition);
        scroller.OnSelectionChanged(UpdateSelection);
    }

    protected virtual void UpdateSelection(int index)
    {
        if (Context.SelectedIndex == index)
        {
            return;
        }
        AudioModel.PlaySound(AudioEnum.Pop.Str());
        Context.SelectedIndex = index;
        Refresh();

        onSelectionChanged?.Invoke(index);
    }

    public override void UpdateContents(IList<TInfo> items)
    {
        base.UpdateContents(items);
        scroller.SetTotalCount(items.Count);
    }

    public void OnSelected(Action<int> callback)
    {
        onSelectionChanged = callback;
    }

    public void SelectNextCell()
    {
        SelectCell(Context.SelectedIndex + 1);
    }

    public void SelectPrevCell()
    {
        SelectCell(Context.SelectedIndex - 1);
    }

    public void SelectCell(int index)
    {
        if (index < 0 || index >= ItemsSource.Count || index == Context.SelectedIndex)
        {
            return;
        }

        UpdateSelection(index);
        scroller.ScrollTo(index, 0.35f, Ease.OutCubic);
    }

    public TCell[] GetCells()
    {
        return cellContainer.GetComponentsInChildren<TCell>(false);
    }
}