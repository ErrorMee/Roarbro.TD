using TMPro;
using UnityEngine;
using DG.Tweening;

public class MsgItem : MonoBehaviour
{
    public TextMeshProUGUI text;

    public CanvasGroup canvasGroup;

    public RectTransform rectTransform;

    Sequence tweenSequence;

    public void Show(string msg, SmartList<MsgItem> msgItems)
    {
        AudioModel.PlaySound(AudioEnum.Pop.Str());

        if (tweenSequence != null)
        {
            tweenSequence.onUpdate = tweenSequence.onComplete = null;
            tweenSequence = null;
        }
        
        rectTransform.localPosition = new Vector3(0, MsgWindow.InitY, 0);

        text.text = msg;
        float alpha = 0;
        canvasGroup.alpha = alpha;
        
        tweenSequence = DOTween.Sequence();

        Tweener tweenIn = DOTween.To(() => alpha, x => alpha = x, 1, 0.5f);
        tweenSequence.Append(tweenIn);
        tweenSequence.AppendInterval(2.0f);
        Tweener tweenOut = DOTween.To(() => alpha, x => alpha = x, 0, 0.5f);
        tweenSequence.Append(tweenOut);

        tweenSequence.onUpdate = () => { canvasGroup.alpha = alpha; };

        tweenSequence.onComplete = () => 
        { 
            GameObjectPool.Instance.Cache(gameObject);
            msgItems.Remove(this);
        };
    }
}
