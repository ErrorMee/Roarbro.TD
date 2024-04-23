using System;
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

        Array terrainValues = Enum.GetValues(typeof(TerrainEnum));
        if (config.colors == null || config.colors.Length != terrainValues.Length)
        {
            config.colors = new Color[terrainValues.Length];
            config.colors[(byte)TerrainEnum.Water] = new Color(0.43f, 0.58f, 0.65f);
            config.colors[(byte)TerrainEnum.Land] = new Color(0.65f, 0.65f, 0.44f);
        }

        EditorGUILayout.BeginHorizontal(GUILayout.Width(xWidth));
        foreach (byte value in Enum.GetValues(typeof(TerrainEnum)))
        {
            DrawPreferredLabel(((TerrainEnum)value).ToString());
            config.colors[value] = EditorGUILayout.ColorField(config.colors[value], GUILayout.Width(40));
        }
        EditorGUILayout.EndHorizontal();

        if (config.terrains == null || config.terrains.Length != GridUtil.AllCount)
        {
            Debug.LogError("new config.terrains");
            config.terrains = new TerrainEnum[GridUtil.AllCount];
            for (int i = 0; i < config.terrains.Length; i++)
            {
                config.terrains[i] = TerrainEnum.Water;
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
                GUI.color = config.colors[(byte)terrain];
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