using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleModel : Singleton<BattleModel>, IDestroy
{
    private GameObject world;

    private List<Transform> battleLayers;

    private List<IDestroy> models = new List<IDestroy>();

    public BattleInfo battle;

    public void Start(BattleInfo battleInfo, bool edit = false)
    {
        battle = battleInfo;
        battle.edit = edit;
        models.Clear();
        models.Add(BattleTimerModel.Instance.Init());

        if (battle.edit == true)
        {
            WindowModel.Open(WindowEnum.BattleEdit);
        }
        else
        {
            WindowModel.Open(WindowEnum.Battle);
            models.Add(PieceModel.Instance.Init());
        }

        world = new GameObject("World");
        battleLayers = new List<Transform>();
        int depth = -2;
        CreateLayer(typeof(TerrainLayer), depth++);
        if (battle.edit == false)
        {
            CreateLayer(typeof(PieceLayer), depth++);
        }
        else
        {
            CreateLayer(typeof(EnemyLayer), depth++);
        }
    }

    private void CreateLayer(Type type, int depth)
    {
        GameObject battleLayer = new(type.Name);
        battleLayer.transform.SetParent(world.transform, false);
        battleLayer.AddComponent(type);
        battleLayer.transform.position = new Vector3(0, depth * 0.01f, 0);
        battleLayers.Add(battleLayer.transform);
    }

    public void Complete()
    {
        StageModel.Instance.Complete();
    }

    public void DestroyBattle()
    {
        if (world != null && CameraModel.Instance != null)
        {
            CameraModel.Instance.Target = null;
            UnityEngine.Object.DestroyImmediate(world);
        }
        world = null;

        for (int i = 0; i < models.Count; i++)
        {
            models[i].Destroy();
        }
        models.Clear();
    }
}