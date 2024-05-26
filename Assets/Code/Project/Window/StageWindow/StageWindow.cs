using UnityEngine;

public class StageWindow : WindowBase
{
    [SerializeField] StageList battleList = default;

    [SerializeField] SDFBtn editBtn;

    [SerializeField] SDFBtn startBtn;

    protected override void Awake()
    {
        base.Awake();

        ClickListener.Add(editBtn.transform).onClick += OnClickEdit;
        ClickListener.Add(startBtn.transform).onClick += OnClickStart;

        battleList.OnSelected((index) =>
        {
            battleList.SelectCell(index);
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
        battleList.UpdateContents(StageModel.Instance.battles);
        battleList.SelectCell(StageModel.Instance.SelectIndex);
    }

    void OnClickEdit()
    {
        BattleModel.Instance.Start(StageModel.Instance.battles[StageModel.Instance.SelectIndex], true);
        BattleModel.Instance.CreateLayer(typeof(TerrainLayer));
        if (BattleModel.Instance.battle.edit == false)
        {
            BattleModel.Instance.CreateLayer(typeof(PieceLayer));
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
                BattleModel.Instance.CreateLayer(typeof(PieceLayer));
            }
        }
        else
        {
            WindowModel.Msg(LanguageModel.Get(10025));
        }
    }
}