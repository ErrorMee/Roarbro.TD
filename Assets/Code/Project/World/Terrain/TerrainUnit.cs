using UnityEngine;

public class TerrainUnit : MonoBehaviour
{
    [ReadOnlyProperty]
    public Vector2Int index;
    public MeshRenderer meshRenderer;

    public void UpdateShow()
    {
        TerrainConfig terrainConfig = BattleModel.Instance.battle.terrain;
        TerrainEnum terrain = terrainConfig.GetTerrain(index.x, index.y);
        Color color = terrainConfig.GetColor(terrain);
        meshRenderer.SetMPBInt(MatPropUtil.IndexKey, (int)terrain, false);
        //meshRenderer.SetMPBColor(MatPropUtil.AddColorKey, new Color(0.35f, 0.35f, 0.4f), false);
        meshRenderer.SetMPBColor(MatPropUtil.AddColorKey, color * 1.25f, false);
        meshRenderer.SetMPBColor(MatPropUtil.BaseColorKey, color);
    }
}