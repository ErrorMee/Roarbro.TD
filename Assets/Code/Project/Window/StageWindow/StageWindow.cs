using UnityEngine;

public class StageWindow : WindowBase
{
    [SerializeField] StageFocus stageFocus = default;

    [SerializeField] SDFBtn editBtn;

    [SerializeField] SDFBtn startBtn;

    protected override void Awake()
    {
        base.Awake();

        ClickListener.Add(editBtn.transform).onClick += OnClickEdit;
        ClickListener.Add(startBtn.transform).onClick += OnClickStart;

        stageFocus.OnSelected((index) =>
        {
            StageModel.Instance.SelectIndex = index;
            startBtn.interactable = StageModel.Instance.IsUnLock(index);
        });

    }

    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);

        StageModel.Instance.Init();

        ShowBattles();
    }

    void ShowBattles()
    {
        stageFocus.UpdateContents(StageModel.Instance.battles);
        stageFocus.SelectCell(StageModel.Instance.SelectIndex);
    }

    void OnClickEdit()
    {
        BattleModel.Instance.Start(StageModel.Instance.battles[StageModel.Instance.SelectIndex], true);
        BattleModel.Instance.CreateLayer(typeof(TerrainLayer));
        if (BattleModel.Instance.battle.edit == false)
        {
            BattleModel.Instance.CreateLayer(typeof(BallLayer));
        }
    }

    void OnClickStart()
    {
        if (StageModel.Instance.IsUnLock(StageModel.Instance.SelectIndex))
        {
            BattleModel.Instance.Start(StageModel.Instance.battles[StageModel.Instance.SelectIndex]);

            BattleModel.Instance.CreateLayer(typeof(TerrainLayer));
            if (BattleModel.Instance.battle.edit == false)
            {
                BattleModel.Instance.CreateLayer(typeof(BallLayer));
            }
        }
        else
        {
            WindowModel.Msg(LanguageModel.Get(10025));
        }
    }
}