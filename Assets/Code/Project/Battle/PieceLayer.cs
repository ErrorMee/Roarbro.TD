
using UnityEngine;

public class PieceLayer : BattleLayer<PieceUnit>
{
    PieceUnit[,] units;

    protected override void Awake()
    {
        base.Awake();

        units = new PieceUnit[GridUtil.XCount, GridUtil.YCount];

        Init();
        OnChangeTerrain();
    }

    private void Init()
    {
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                PieceUnit unit = CreateUnit();
                units[x, y] = unit;
                unit.posx = x;
                unit.posy = y;
                unit.transform.localPosition = new Vector3(x - GridUtil.XRadiusCount, 0, y - GridUtil.YRadiusCount);
            }
        }
    }

    private void OnChangeTerrain(object obj = null)
    {
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                PieceUnit unit = units[x, y];
                unit.UpdateShow();
            }
        }
    }
}