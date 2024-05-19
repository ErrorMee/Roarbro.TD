public partial class EnemyLayer : WorldLayer<EnemyUnit>
{
    public FSM<EnemyLayerState> fsm;

    public EnemyUnit[,] units;

    protected override void Awake()
    {
        base.Awake();
        fsm = new FSM<EnemyLayerState>();
        fsm.AddState(EnemyLayerState.Edit, EditEnter, EditUpdate);
        fsm.AddState(EnemyLayerState.Move, MoveEnter, MoveUpdate);

        if (BattleModel.Instance.battle.edit)
        {
            fsm.ChangeState(EnemyLayerState.Edit);
        }
        else
        {
            fsm.ChangeState(EnemyLayerState.Move);
        }

        AutoListener(EventEnum.EnemyDie, OnEnemyDie);
    }

    private void Update()
    {
        fsm.Update();
    }

    void OnEnemyDie(object obj)
    {
        EnemyUnit dieUnit = (EnemyUnit)obj;
        units[dieUnit.info.index.x, dieUnit.info.index.y] = null;
        GridModel.Instance.RemoveItem(dieUnit);
        Destroy(dieUnit.gameObject);

        EnemyModel.Instance.EnemyDie(dieUnit.info);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GridModel.Instance.DeletetGrid<EnemyUnit>();
    }
}