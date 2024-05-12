using UnityEngine;
using TMPro;
using DG.Tweening;

public class DialogWindow : WindowBase
{
    [SerializeField]
    TextMeshProUGUI title;

    [SerializeField]
    TextMeshProUGUI content;

    [SerializeField]
    SDFBtn cancelBtn;

    [SerializeField]
    SDFBtn confirmBtn;

    DialogInfo info;

    protected override void Awake()
    {
        base.Awake();
        
        ClickListener.Add(cancelBtn.transform).onClick = OnCancel;
        ClickListener.Add(confirmBtn.transform).onClick = OnConfirm;
    }

    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);

        cancelBtn.transform.localScale = Vector3.zero;
        confirmBtn.transform.localScale = Vector3.zero;
        cancelBtn.transform.DOScale(1, 0.2f).SetDelay(0.25f);
        confirmBtn.transform.DOScale(1, 0.2f).SetDelay(0.25f);

        info = (DialogInfo)obj;
        Show();
    }

    private void Show()
    {
        title.text = info.title;
        content.text = info.content;

        cancelBtn.gameObject.SetActive(info.cancel != null);
        confirmBtn.gameObject.SetActive(info.confirm != null);

        AirModel.Add(transform, OnClickAir);
    }

    private void OnConfirm()
    {
        info.confirm?.Invoke();
        CloseSelf();
    }

    private void OnCancel()
    {
        info.cancel?.Invoke();
        CloseSelf();
    }
    
    private void OnClickAir()
    {
        if(info.cancel != null)
        {
            OnCancel();
        }
    }

    protected override void OnDisable()
    {
        SharedPool<DialogInfo>.Cache(info);
    }
}