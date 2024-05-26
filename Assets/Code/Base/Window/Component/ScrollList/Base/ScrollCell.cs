using FancyScrollView;
using UnityEngine;
[DisallowMultipleComponent]
public class ScrollCell<TInfo> : FancyGridViewCell<TInfo, ScrollContext>
{
    [SerializeField] protected SDFBtn btn = default;

    public override void Initialize()
    {
        if (btn != null)
        {
            ClickListener.Add(btn.transform).onClick = OnClick;
        }
    }

    void OnClick()
    {
        Context.OnCellClicked?.Invoke(Index);
    }

    public override void UpdateContent(TInfo info)
    {
    }
}
