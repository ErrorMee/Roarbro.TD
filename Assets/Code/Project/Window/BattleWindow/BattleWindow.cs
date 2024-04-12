using TMPro;
using UnityEngine;

public class BattleWindow : WindowBase
{
    [SerializeField] SDFBtn pauseBtn;

    private void OnApplicationFocus(bool focus)
    {
        if (focus == false && BattleTimerModel.Instance.CountDown > 0)
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

        BattleTimerModel.AddImmediately(OnTick, TimerEnum.Tick);
        BattleTimerModel.AddImmediately(OnLogic, TimerEnum.Logic);
    }

    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);
        BattleTimerModel.Countinue();
        CameraModel.Instance.CullingMaskAll();
        OnTick();
    }

    private void OnTick()
    {

    }

    private void OnLogic()
    {
        
    }

    void OnClickPause()
    {
        CloseSelf();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        BattleModel.Instance.CloseBattle();
    }
}
