using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleWindow : WindowBase
{
    [SerializeField] SDFBtn pauseBtn;
    [SerializeField] TextMeshProUGUI step;

    [SerializeField] TextMeshProUGUI hpTxt;
    [SerializeField] Slider hpSlider;

    override protected void Awake()
    {
        base.Awake();

        ClickListener.Add(pauseBtn.transform).onClick += OnClickPause;

        AutoListener(EventEnum.ChangeStep, OnChangeStep);
        OnChangeStep();
        AutoListener(EventEnum.ChangeHP, OnChangeHP);
        OnChangeHP();
    }

    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);
        CameraModel.Instance.CullingMaskAll();
    }

    void OnChangeStep(object obj = null)
    {
        step.text = BallModel.Instance.LeftStep + "/" + BallModel.Instance.MaxStep;
    }

    void OnChangeHP(object obj = null)
    {
        hpSlider.minValue = 0;
        hpSlider.maxValue = BattleModel.Instance.battle.maxHP;
        hpSlider.value = BattleModel.Instance.battle.leftHP;
        if (hpSlider.value < 15)
        {
            hpSlider.value = 15;
        }
        hpTxt.text = BattleModel.Instance.battle.leftHP + "/" + BattleModel.Instance.battle.maxHP;
    }

    void OnClickPause()
    {
        Open(WindowEnum.Pause);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (ProjectConfigs.isQuiting == false)
        {
            BattleModel.Instance.DestroyBattle();
        }
    }
}
