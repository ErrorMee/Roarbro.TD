using FancyScrollView;
using UnityEngine;

public class ScrollFocusCell<TInfo> : FancyCell<TInfo, ScrollFocusContext> where TInfo : class
{
    [SerializeField] Animator animator = default;

    static class AnimatorHash
    {
        public static readonly int Scroll = Animator.StringToHash("scroll");
    }

    public override void Initialize()
    {
    }

    public override void UpdateContent(TInfo itemData)
    {
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
