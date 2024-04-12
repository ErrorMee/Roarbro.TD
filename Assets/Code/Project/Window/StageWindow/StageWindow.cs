using DG.Tweening;
using UnityEngine;

public class StageWindow : WindowBase
{
    [SerializeField] StageList battleList = default;

    [SerializeField] SDFBtn editBtn;

    [SerializeField] SDFBtn playBtn;

    protected override void Awake()
    {
        base.Awake();

        ClickListener.Add(editBtn.transform).onClick += OnClickEdit;
        ClickListener.Add(playBtn.transform).onClick += OnClickPlay;

        battleList.OnCellClicked((index) =>
        {
            battleList.UpdateSelection(index);
            StageModel.Instance.SelectIndex = index;
            playBtn.interactable = StageModel.Instance.IsUnLock(index);
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
        battleList.UpdateSelection(StageModel.Instance.SelectIndex);
    }

    void OnClickEdit()
    {
        BattleModel.Instance.Start(StageModel.Instance.battles[StageModel.Instance.SelectIndex], true);
    }

    void OnClickPlay()
    {
        if (StageModel.Instance.IsUnLock(StageModel.Instance.SelectIndex))
        {
            BattleModel.Instance.Start(StageModel.Instance.battles[StageModel.Instance.SelectIndex]);
        }
        else
        {
            WindowModel.Msg(LanguageModel.Get(10025));
        }
    }
}