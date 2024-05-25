using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ArmyConfigs))]
public class ArmyConfigsEditor : ConfigsEditor<ArmyConfig, ArmyConfigs>
{
    [MenuItem(ConfigMenu + "Army")]
    static void Create()
    {
        CreateConfigs<ArmyConfigs>("Army");
    }

    protected override void DrawHead()
    {
        base.DrawHead();
        if (EnemyConfigs.Instance == null)
        {
            EnemyConfigs.Instance = GetConfigs("Enemy") as EnemyConfigs;
        }
    }

    protected override void DrawMenuAdd()
    {
        base.DrawMenuAdd();
        GotoConfigs("Battle", 45);
        OptStrUtil.Init();
    }

    protected override void DrawConfig(int index, ArmyConfig config)
    {
        base.DrawConfig(index, config);

        int xCount = GridUtil.XCount;
        int zCount = GridUtil.YCount;
        int tileWidth = 18; int xWidth = (xCount + 2) * tileWidth;
        EditorGUILayout.BeginVertical(GUILayout.Width(xWidth));

        if (config.enemys == null || config.enemys.Length != GridUtil.AllCount)
        {
            Debug.LogError("new ArmyEnemyConfig");
            config.enemys = new EnemyTemplate[GridUtil.AllCount];
            for (int i = 0; i < config.enemys.Length; i++)
            {
                config.enemys[i] = new EnemyTemplate();
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
                EnemyTemplate enemy = config.enemys[GridUtil.GetIndex(x, z)];

                oriColor = GUI.color;
                GUI.color = EnemyConfigs.Instance.GetConfigByID(enemy.enemyID).color;
                EditorGUILayout.LabelField(enemy.level.OptStr(), GUILayout.Width(tileWidth));
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