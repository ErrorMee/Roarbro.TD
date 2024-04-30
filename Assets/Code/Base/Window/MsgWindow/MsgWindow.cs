using DG.Tweening;

public class MsgWindow : WindowBase
{
    public MsgItem msgTemplate;

    const int maxCount = 6;

    private readonly SmartList<MsgItem> msgItems = new(maxCount);

    float itemGap;

    override protected void Awake()
    {
        base.Awake();
        msgTemplate.gameObject.SetActive(false);
        GameObjectPool.Instance.Init(msgTemplate.gameObject);
        itemGap = msgTemplate.rectTransform.sizeDelta.y;
    }

    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);

        MsgItem msgInstance = GameObjectPool.Instance.Get<MsgItem>(msgTemplate.gameObject);
        msgItems.PushPop(msgInstance);

        DOTween.Kill(msgInstance.transform, false);
        msgInstance.Show(obj.ToString(), msgItems);

        int len = 0;
        for (int i = msgItems.Count - 2; i >= 0; i--)
        {
            MsgItem msgItem = msgItems[i];
            len++;
            msgItem.transform.DOLocalMoveY(len * itemGap, 0.2f);
        }
    }
}
