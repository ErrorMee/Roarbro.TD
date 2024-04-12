using System;
using System.Collections.Generic;
using UnityEngine;

public class ScrollFocusContext
{
    public int SelectedIndex = -1;
    public Action<int> OnCellClicked;
}