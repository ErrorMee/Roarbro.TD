using FancyScrollView;
using System;
public class ScrollContext : FancyGridViewContext
{
    public int SelectedIndex = -1;
    public Action<int> OnCellClicked;
}