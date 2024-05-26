using FancyScrollView;
using UnityEngine;

public class ScrollFocusCell<TInfo> : FancyCell<TInfo, ScrollFocusContext> where TInfo : class
{
    [SerializeField] protected SDFBtn btn = default;
    [SerializeField] protected SDFImg select = default;
    [SerializeField] Animator animator = default;

    static class AnimatorHash
    {
        public static readonly int Scroll = Animator.StringToHash("scroll");
    }

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

    public override void UpdateContent(TInfo itemData)
    {
        select.gameObject.SetActive(Index == Context.SelectedIndex);
    }

    public override void UpdatePosition(float position)
    {
        currentPosition = position;

        if (animator.isActiveAndEnabled)
        {
            animator.Play(AnimatorHash.Scroll, -1, position);
        }

        animator.speed = 0;
    }

    // GameObject が非アクティブになると Animator がリセットされてしまうため
    // 現在位置を保持しておいて OnEnable のタイミングで現在位置を再設定します
    float currentPosition = 0;

    void OnEnable() => UpdatePosition(currentPosition);
}
