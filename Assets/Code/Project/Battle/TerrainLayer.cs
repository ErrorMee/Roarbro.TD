using UnityEngine;

public class TerrainLayer : BattleLayer<TileUnit>
{
    TileUnit[,] units;

    protected override void Awake()
    {
        base.Awake();

        units = new TileUnit[GridUtil.XCount, GridUtil.YCount];

        AutoListener(EventEnum.ResetTerrain, OnChangeTerrain);
        Init();
        OnChangeTerrain();
    }

    private void Init()
    {
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                TileUnit unit = CreateUnit();
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
                TileUnit unit = units[x, y];
                unit.UpdateShow();
            }
        }
    }
}