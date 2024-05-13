using TMPro;

public partial class EnemyUnit : WorldUnit
{
    public FSM<EnemyState> fsm;

    [ReadOnlyProperty]
    public EnemyInfo info;
    public TextMeshPro txt;

    private void Awake()
    {
        fsm = new FSM<EnemyState>();
        fsm.mStates.Add(new State<EnemyState>(EnemyState.Idle, IdleEnter, IdleUpdate));
        fsm.mStates.Add(new State<EnemyState>(EnemyState.Move, MoveEnter, MoveUpdate));
    }

    protected override void OnLogic()
    {
        base.OnLogic();
        fsm.Update();
    }

    public void UpdateShow()
    {
        meshRenderer.SetMPBInt(MatPropUtil.IndexKey, info.config.avatar, false);
        meshRenderer.SetMPBColor(MatPropUtil.BaseColorKey, info.config.color);

        if (info.enemyInfoConfig.enemyID > 0 && info.enemyInfoConfig.level > 1)
        {
            txt.text = info.enemyInfoConfig.level.OptStr();
        }
        else
        {
            txt.text = string.Empty;
        }
    }
}