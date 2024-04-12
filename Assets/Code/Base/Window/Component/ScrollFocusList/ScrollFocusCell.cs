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

    // GameObject ���ǥ����ƥ��֤ˤʤ�� Animator ���ꥻ�åȤ���Ƥ��ޤ�����
    // �F��λ�ä򱣳֤��Ƥ����� OnEnable �Υ����ߥ󥰤ǬF��λ�ä����O�����ޤ�
    float currentPosition = 0;

    void OnEnable() => UpdatePosition(currentPosition);
}
