using DG.Tweening;

public class MsgWindow : WindowBase
{
    public MsgItem msgItemPrefab;

    const int maxCount = 5;

    private readonly SmartList<MsgItem> msgItems = new(maxCount);

    float itemGap;

    public const int InitY = 128;

    override protected void Awake()
    {
        base.Awake();
        msgItemPrefab.gameObject.SetActive(false);
        GameObjectPool.Instance.Init(msgItemPrefab.gameObject);
        itemGap = msgItemPrefab.rectTransform.sizeDelta.y;
    }

    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);

        MsgItem msgInstance = GameObjectPool.Instance.Get<MsgItem>(msgItemPrefab.gameObject);
        msgItems.PushPop(msgInstance);

        DOTween.Kill(msgInstance.transform, false);
        msgInstance.Show(obj.ToString(), msgItems);

        int len = 0;
        for (int i = msgItems.Count - 2; i >= 0; i--)
        {
            MsgItem msgItem = msgItems[i];
            len++;
            msgItem.transform.DOLocalMoveY(InitY + len * itemGap, 0.2f);
        }
    }
}
