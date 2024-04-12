using UnityEngine;

public class TileUnit : BattleUnit
{
    [ReadOnlyProperty]
    public int posx;
    [ReadOnlyProperty]
    public int posy;

    protected override void OnEnable() { }
    protected override void OnDisable() { }

    public void UpdateShow()
    {
        BattleConfig battleConfig = BattleModel.Instance.battle.config;

        TerrainEnum terrain = battleConfig.terrains[GridUtil.GetIndex(posx, posy)];
        Color color = terrain.GetColor();
        meshRenderer.SetMPBInt(MatPropUtil.IndexKey, (int)terrain, false);
        //meshRenderer.SetMPBColor(MatPropUtil.AddColorKey, new Color(0.35f, 0.35f, 0.4f), false);
        meshRenderer.SetMPBColor(MatPropUtil.AddColorKey, color * 1.25f, false);
        meshRenderer.SetMPBColor(MatPropUtil.BaseColorKey, color);
    }
}