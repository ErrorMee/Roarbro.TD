using UnityEngine;

public partial class BallLayer : WorldLayer<BallUnit>
{
    FSM<BallLayerState> fsm = new FSM<BallLayerState>();

    public BallUnit[,] units;
    public BallUnit selectUnit;

    protected override void Awake()
    {
        base.Awake();
        
        Init();

        AutoListener(EventEnum.MoveBall, OnMoveBall);
    }

    private void Init()
    {
        fsm.AddState(BallLayerState.Idle, IdleEnter, IdleUpdate);
        fsm.AddState(BallLayerState.Drag, DragEnter, DragUpdate);
        fsm.AddState(BallLayerState.Move, MoveEnter, MoveUpdate);
        fsm.AddState(BallLayerState.Merge, MergeEnter, MergeUpdate);
        fsm.AddState(BallLayerState.Fill, FillEnter, FillUpdate);
        fsm.AddState(BallLayerState.Fight, FightEnter, FightUpdate);

        units = new BallUnit[GridUtil.XCount, GridUtil.YCount];
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                BallUnit unit = CreateUnit();
                units[x, y] = unit;
                unit.info = BallModel.Instance.infos[x, y];
                unit.UpdateShow();
                unit.transform.localPosition = 
                    new Vector3(unit.info.GetViewX(), 0, unit.info.GetViewZ() - GridUtil.YCount);
                moveBalls.Add(unit);
            }
        }

        fsm.ChangeState(BallLayerState.Move);
    }

    private BallUnit GetUnit(BallInfo info)
    {
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                BallUnit unit = units[x, y];
                if (unit.info == info)
                {
                    return unit;
                }
            }
        }
        return null;
    }

    private void Update()
    {
        fsm.Update();
    }
}