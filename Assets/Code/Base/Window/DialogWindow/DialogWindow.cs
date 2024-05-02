using UnityEngine;
using TMPro;

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
        
        info = (DialogInfo)obj;
        Show();
    }

    private void Show()
    {
        title.text = info.title;
        content.text = info.content;

        cancelBtn.gameObject.SetActive(info.cancel != null);
        confirmBtn.gameObject.SetActive(info.confirm != null);

        if (info.airEnum != AirEnum.None)
        {
            if (AirModel.Instance.airStack.Count > 0)
            {
                if (AirModel.Instance.airStack.Peek() == info.airEnum)
                {
                    AirModel.Add(transform, OnClickAir, AirEnum.Alpha);
                } else
                {
                    AirModel.Add(transform, OnClickAir, info.airEnum);
                }
            }
            else
            {
                AirModel.Add(transform, OnClickAir, info.airEnum);
            }
        }
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
        if (info.airEnum != AirEnum.None)
        {
            AirModel.Remove(transform);
        }
        SharedPool<DialogInfo>.Cache(info);
    }
}