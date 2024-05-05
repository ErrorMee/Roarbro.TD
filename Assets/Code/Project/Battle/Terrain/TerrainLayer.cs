using UnityEngine;

public partial class TerrainLayer : BattleLayer<TerrainUnit>
{
    TerrainUnit[,] units;

    protected override void Awake()
    {
        base.Awake();

        units = new TerrainUnit[GridUtil.XCount, GridUtil.YCount];

        AutoListener(EventEnum.ResetTerrain, OnChangeTerrain);
        Init();
        OnChangeTerrain();

        CreateSelect();
    }

    private void Init()
    {
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                TerrainUnit unit = CreateUnit();
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
                TerrainUnit unit = units[x, y];
                unit.UpdateShow();
            }
        }
    }
}