using UnityEngine;

public partial class TerrainLayer : WorldLayer<TerrainUnit>
{
    TerrainUnit[,] units;

    protected override void Awake()
    {
        base.Awake();

        Init();
        UpdateUnits();
        CreateSelect();
    }

    private void Init()
    {
        units = new TerrainUnit[GridUtil.XCount, GridUtil.YCount];
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                TerrainUnit unit = CreateUnit();
                units[x, y] = unit;
                unit.index.x = x;
                unit.index.y = y;
                unit.transform.localPosition = new Vector3(x - GridUtil.XRadiusCount, 0, y - GridUtil.YRadiusCount);
            }
        }
    }

    private void UpdateUnits(object obj = null)
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