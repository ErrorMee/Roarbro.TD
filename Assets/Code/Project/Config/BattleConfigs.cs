
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BattleConfigs : Configs<BattleConfigs, BattleConfig>
{
    public void Save()
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssetIfDirty(this);
#endif
    }
}

[Serializable]
public class BattleConfig : Config
{
    public TerrainEnum terrainSelect = TerrainEnum.Ground;
    public TerrainEnum[] terrains;
}