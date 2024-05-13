public partial class EnemyLayer : WorldLayer<EnemyUnit>
{
    public FSM<EnemyLayerState> fsm;

    public EnemyUnit[,] units;

    protected override void Awake()
    {
        base.Awake();
        fsm = new FSM<EnemyLayerState>();
        fsm.mStates.Add(new State<EnemyLayerState>(EnemyLayerState.Edit, EditEnter, EditUpdate));
        fsm.mStates.Add(new State<EnemyLayerState>(EnemyLayerState.Move, MoveEnter, MoveUpdate));

        units = new EnemyUnit[GridUtil.XCount, GridUtil.YCount];
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                EnemyUnit unit = CreateUnit();
                units[x, y] = unit;
                unit.info = EnemyModel.Instance.infos[x, y];
            }
        }

        if (BattleModel.Instance.battle.edit)
        {
            fsm.ChangeState(EnemyLayerState.Edit);
        }
        else
        {
            fsm.ChangeState(EnemyLayerState.Move);
        }
    }

    private void Update()
    {
        fsm.Update();
    }
}