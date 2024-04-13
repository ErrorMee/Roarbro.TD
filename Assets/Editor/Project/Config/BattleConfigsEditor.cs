using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BattleConfigs))]
public class BattleConfigsEditor : ConfigsEditor<BattleConfig, BattleConfigs>
{
    [MenuItem(ConfigMenu + "Battle")]
    static void Create()
    {
        CreateConfigs<BattleConfigs>("Battle");
    }

    int[] terrainInts;
    string[] terrainStrs;

    protected override void DrawHead()
    {
        base.DrawHead();

        if (TerrainConfigs.Instance == null)
        {
            TerrainConfigs.Instance = GetConfigs("Terrain") as TerrainConfigs;
        }
        if (terrainInts == null)
        {
            terrainInts = new int[TerrainConfigs.Instance.all.Length];
            terrainStrs = new string[terrainInts.Length];

            for (int i = 0; i < terrainInts.Length; i++)
            {
                TerrainConfig terrain = TerrainConfigs.Instance.all[i];
                terrainInts[i] = terrain.id;
                terrainStrs[i] = terrainInts[i].ToString();
            }
        }
        GotoConfigs("Terrain", 50);
    }

    protected override void DrawMenuAdd()
    {
        base.DrawMenuAdd();
    }

    protected override void DrawConfig(int index, BattleConfig config)
    {
        base.DrawConfig(index, config);

        config.terrain = EditorGUILayout.IntPopup(config.terrain, terrainStrs, terrainInts, GUILayout.Width(50));

    }
}