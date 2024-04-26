using UnityEngine;

public partial class PieceLayer : BattleLayer<PieceUnit>
{
    public FSM<PieceLayerState> fsm;

    public PieceUnit[,] units;
    public PieceUnit selectUnit;
    public TileUnitSelect select;

    protected override void Awake()
    {
        base.Awake();
        select = AddressModel.LoadGameObject(
            Address.UnitPrefab(typeof(TileUnitSelect).Name), transform).GetComponent<TileUnitSelect>();
        select.gameObject.SetActive(false);

        Init();

        AutoListener(EventEnum.MovePiece, OnAddMovePiece);
    }

    private void Init()
    {
        fsm = new FSM<PieceLayerState>();

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
                unit.info = PieceModel.Instance.pieceInfos[x, y];
                unit.UpdateShow();
                unit.transform.localPosition = 
                    new Vector3(unit.info.GetViewX(), 0, unit.info.GetViewZ() - GridUtil.YCount);
                movePieces.Add(unit);
            }
        }

        ChangeState(PieceLayerState.Move);
    }

    private PieceUnit GetPieceUnit(PieceInfo pieceInfo)
    {
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                PieceUnit pieceUnit = units[x, y];
                if (pieceUnit.info == pieceInfo)
                {
                    return pieceUnit;
                }
            }
        }
        return null;
    }

    private void ChangeState(PieceLayerState pieceLayerState)
    {
        foreach (var item in fsm.mStates)
        {
            if (item.mInfo == pieceLayerState)
            {
                fsm.ChangeState(item);
                return;
            }
        }
    }

    private void Update()
    {
        fsm.Update();
    }
}