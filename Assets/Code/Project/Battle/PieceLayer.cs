using DG.Tweening;
using UnityEngine;

public class PieceLayer : BattleLayer<PieceUnit>
{
    PieceUnit[,] units;
    PieceUnit selectUnit;
    TileUnitSelect select;

    bool start = false;

    protected override void Awake()
    {
        base.Awake();
        select = AddressModel.LoadGameObject(
            Address.UnitPrefab(typeof(TileUnitSelect).Name), transform).GetComponent<TileUnitSelect>();
        select.gameObject.SetActive(false);

        units = new PieceUnit[GridUtil.XCount, GridUtil.YCount];
        DOVirtual.DelayedCall(0.5f, () => { start = true; });
        Init();

        AutoListener(EventEnum.MovePiece, OnMovePiece);
    }

    private void Init()
    {
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                PieceUnit unit = CreateUnit();
                units[x, y] = unit;
                unit.info = PieceModel.Instance.pieceInfos[x, y];
                unit.UpdateShow();
            }
        }
    }

    private void Update()
    {
        if (start && InputModel.Instance.PressedThisFrame)
        {
            Select();
        }

        if (selectUnit != null)
        {
            if (InputModel.Instance.Presseding)
            {
                Drag();
            }
            if (InputModel.Instance.ReleasedThisFrame)
            {
                DragDown();
            }
        }
    }

    void Select()
    {
        Vector2Int index = GetTouchIndex();

        if (GridUtil.InGrid(index.x, index.y))
        {
            selectUnit = units[index.x, index.y];
        }
        else
        {
            selectUnit = null;
        }
    }

    void Drag()
    {
        Vector3 worldPos = CameraModel.Instance.ScreenToWorldPos(InputModel.Instance.Touch0LastPos, transform.position.y);

        float viewX = selectUnit.info.GetViewX();
        float viewZ = selectUnit.info.GetViewZ();

        Vector3 unitPos = new Vector3(Mathf.Clamp(worldPos.x, viewX - 1f, viewX + 1f),
            0.02f, Mathf.Clamp(worldPos.z, viewZ - 1f, viewZ + 1f));

        Vector3 offset = unitPos - selectUnit.transform.localPosition;
        if (offset.sqrMagnitude < 0.04f)
        {
            selectUnit.transform.localPosition = unitPos;
        }
        else
        {
            selectUnit.transform.localPosition += offset.normalized * 0.2f;
        }
        
        Vector3 selectPos = GridUtil.WorldToGridPos(unitPos, false);
        Vector2Int index = GridUtil.WorldToGridIndex(selectPos);
        if (GridUtil.InGrid(index.x, index.y))
        {
            if ((Mathf.Abs(index.x - selectUnit.info.index.x) + Mathf.Abs(index.y - selectUnit.info.index.y)) > 1)
            {
                select.meshRenderer.SetMPBColor(MatPropUtil.BaseColorKey, QualityConfigs.GetColor2(QualityEnum.SSR));
            }
            else
            {
                select.meshRenderer.SetMPBColor(MatPropUtil.BaseColorKey, QualityConfigs.GetColor(QualityEnum.N));
            }
        }
        else
        {
            select.meshRenderer.SetMPBColor(MatPropUtil.BaseColorKey, QualityConfigs.GetColor2(QualityEnum.SSR));
        }
        selectPos.y = 0;
        select.transform.localPosition = selectPos;
    }

    void DragDown()
    {
        select.gameObject.SetActive(false);
        Vector2Int index = GridUtil.WorldToGridIndex(select.transform.localPosition);
        PieceModel.Instance.DragDown(selectUnit.info, index);
        selectUnit = null;
    }

    Vector2Int GetTouchIndex()
    {
        Vector3 worldPos = CameraModel.Instance.ScreenToWorldPos(InputModel.Instance.Touch0LastPos, transform.position.y);
        worldPos = GridUtil.WorldToGridPos(worldPos, false);
        Vector2Int index = GridUtil.WorldToGridIndex(worldPos);

        select.transform.localPosition = worldPos;
        select.gameObject.SetActive(true);

        return index;
    }

    private void OnMovePiece(object obj = null)
    {
        PieceInfo moveInfo = (PieceInfo)obj;

        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                PieceUnit moveUnit = units[x, y];
                if (moveUnit.info == moveInfo)
                {
                    moveUnit.transform.DOLocalMove(new Vector3(moveInfo.GetViewX(), 0, moveInfo.GetViewZ()), 0.16f);
                    PieceUnit tempUnit = units[moveInfo.index.x, moveInfo.index.y];
                    units[moveInfo.index.x, moveInfo.index.y] = moveUnit;
                    units[x, y] = tempUnit;
                    break;
                }
            }
        }
    }
}