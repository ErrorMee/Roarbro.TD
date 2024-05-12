using UnityEngine;

public partial class PieceLayer : WorldLayer<PieceUnit>
{
    FSM<PieceLayerState> fsm = new FSM<PieceLayerState>();

    public PieceUnit[,] units;
    public PieceUnit selectUnit;

    protected override void Awake()
    {
        base.Awake();
        
        Init();

        AutoListener(EventEnum.MovePiece, OnAddMovePiece);
    }

    private void Init()
    {
        fsm.mStates.Add(new State<PieceLayerState>(PieceLayerState.Idle, IdleEnter, IdleUpdate));
        fsm.mStates.Add(new State<PieceLayerState>(PieceLayerState.Drag, DragEnter, DragUpdate));
        fsm.mStates.Add(new State<PieceLayerState>(PieceLayerState.Move, MoveEnter, MoveUpdate));
        fsm.mStates.Add(new State<PieceLayerState>(PieceLayerState.Merge, MergeEnter, MergeUpdate));
        fsm.mStates.Add(new State<PieceLayerState>(PieceLayerState.Fill, FillEnter, FillUpdate));

        units = new PieceUnit[GridUtil.XCount, GridUtil.YCount];
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                PieceUnit unit = CreateUnit();
                units[x, y] = unit;
                unit.info = PieceModel.Instance.infos[x, y];
                unit.UpdateShow();
                unit.transform.localPosition = 
                    new Vector3(unit.info.GetViewX(), 0, unit.info.GetViewZ() - GridUtil.YCount);
                movePieces.Add(unit);
            }
        }

        fsm.ChangeState(PieceLayerState.Move);
    }

    private PieceUnit GetUnit(PieceInfo info)
    {
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                PieceUnit unit = units[x, y];
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