using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleModel : Singleton<BattleModel>, IDestroy
{
    private GameObject battleObj;

    private List<Transform> battleLayers;

    private List<IDestroy> models = new List<IDestroy>();

    public BattleInfo battle;

    public void Start(BattleInfo battle, bool edit = false)
    {
        this.battle = battle;
        this.battle.edit = edit;
        models.Clear();
        models.Add(BattleTimerModel.Instance.Init());

        if (this.battle.edit)
        {
            WindowModel.Open(WindowEnum.BattleEdit);
        }
        else
        {
            WindowModel.Open(WindowEnum.Battle);
        }

        battleObj = new GameObject("Battle");
        battleLayers = new List<Transform>();
        int depth = -2;
        CreateLayer(typeof(TerrainLayer), depth++);
        CreateLayer(typeof(TouchLayer), depth++);
    }

    private void CreateLayer(Type type, int depth)
    {
        GameObject battleLayer = new(type.Name);
        battleLayer.transform.SetParent(battleObj.transform, false);
        battleLayer.AddComponent(type);
        battleLayer.transform.position = new Vector3(0, depth * 0.01f, 0);
        battleLayers.Add(battleLayer.transform);
    }

    public void CloseBattle()
    {
        if (battleObj != null)
        {
            CameraModel.Instance.Target = null;
            UnityEngine.Object.DestroyImmediate(battleObj);
        }
        battleObj = null;

        for (int i = 0; i < models.Count; i++)
        {
            models[i].Destroy();
        }
        models.Clear();
    }
}