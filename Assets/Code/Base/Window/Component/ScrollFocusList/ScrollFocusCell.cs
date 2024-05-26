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

    // GameObject ���ǥ����ƥ��֤ˤʤ�� Animator ���ꥻ�åȤ���Ƥ��ޤ�����
    // �F��λ�ä򱣳֤��Ƥ����� OnEnable �Υ����ߥ󥰤ǬF��λ�ä����O�����ޤ�
    float currentPosition = 0;

    void OnEnable() => UpdatePosition(currentPosition);
}
