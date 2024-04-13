using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainConfigs))]
public class TerrainConfigsEditor : ConfigsEditor<TerrainConfig, TerrainConfigs>
{
    [MenuItem(ConfigMenu + "Terrain")]
    static void Create()
    {
        CreateConfigs<TerrainConfigs>("Terrain");
    }

    protected override void DrawHead()
    {
        base.DrawHead();
        if (QualityConfigs.Instance == null)
        {
            QualityConfigs.Instance = GetConfigs("Quality") as QualityConfigs;
        }
    }

    protected override void DrawMenuAdd()
    {
        base.DrawMenuAdd();
        GotoConfigs("Battle", 45);
    }

    protected override void DrawConfig(int index, TerrainConfig config)
    {
        base.DrawConfig(index, config);

        int xCount = GridUtil.XCount;
        int zCount = GridUtil.YCount;
        int tileWidth = 18; int xWidth = (xCount + 2) * tileWidth;
        EditorGUILayout.BeginVertical(GUILayout.Width(xWidth));

        if (config.terrains == null || config.terrains.Length != GridUtil.XCount * GridUtil.YCount)
        {
            Debug.LogError("new config.terrains");
            config.terrains = new TerrainEnum[GridUtil.XCount * GridUtil.YCount];
            for (int i = 0; i < config.terrains.Length; i++)
            {
                config.terrains[i] = TerrainEnum.Ground;
            }
        }

        Color oriColor = GUI.color;
        for (int z = zCount - 1; z >= 0; z--)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.Width(xWidth));
            GUI.color = Color.gray;
            EditorGUILayout.LabelField(z.ToString(), GUILayout.Width(tileWidth));
            GUI.color = oriColor;
            for (int x = 0; x < xCount; x++)
            {
                TerrainEnum terrain = config.terrains[GridUtil.GetIndex(x, z)];
                oriColor = GUI.color;
                GUI.color = terrain.GetConfigColor();
                EditorGUILayout.LabelField("¡ö", GUILayout.Width(tileWidth));
                GUI.color = oriColor;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.BeginHorizontal(GUILayout.Width(xWidth));
        GUI.color = Color.gray;
        EditorGUILayout.LabelField("", GUILayout.Width(tileWidth));
        for (int x = 0; x < xCount; x++)
        {
            EditorGUILayout.LabelField(x.ToString(), GUILayout.Width(tileWidth));
        }
        GUI.color = oriColor;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }
}