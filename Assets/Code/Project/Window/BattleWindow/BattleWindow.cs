using TMPro;
using UnityEngine;

public class BattleWindow : WindowBase
{
    [SerializeField] SDFBtn pauseBtn;
    [SerializeField] TextMeshProUGUI step;

    private void OnApplicationFocus(bool focus)
    {
        if (focus == false)
        {
            //if (WindowModel.Get((int)WindowEnum.Pause) == null)
            //{
            //    WindowModel.Open(WindowEnum.Pause);
            //}
        }
    }

    override protected void Awake()
    {
        base.Awake();

        ClickListener.Add(pauseBtn.transform).onClick += OnClickPause;

        AutoListener(EventEnum.ChangeStep, OnChangeStep);
        OnChangeStep();
    }

    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);
        LogicUpdateModel.Instance.timer.Pause = false;
        CameraModel.Instance.CullingMaskAll();
    }

    void OnChangeStep(object obj = null)
    {
        step.text = PieceModel.Instance.LeftStep + "/" + PieceModel.Instance.MaxStep;
    }

    void OnClickPause()
    {
        CloseSelf();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        BattleModel.Instance.DestroyBattle();
    }
}
