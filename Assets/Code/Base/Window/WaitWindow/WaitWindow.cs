using DG.Tweening;
using UnityEngine;

public class WaitWindow : WindowBase
{
    public Transform loading;

    protected override void Awake()
    {
        base.Awake();
        AirModel.Add(transform, null);
    }

    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);
        loading.gameObject.SetActive(false);
        DOVirtual.DelayedCall(0.05f, () => { loading.gameObject.SetActive(true); });
    }
}