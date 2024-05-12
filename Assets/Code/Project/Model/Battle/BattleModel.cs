using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleModel : Singleton<BattleModel>, IDestroy
{
    private HashSet<Type> layers = new HashSet<Type>();

    private List<IDestroy> models = new List<IDestroy>();

    public BattleInfo battle;

    public void Start(BattleInfo battleInfo, bool edit = false)
    {
        battle = battleInfo;
        battle.edit = edit;
        models.Clear();
        models.Add(BattleTimerModel.Instance.Init());
        models.Add(EnemyModel.Instance.Init());
        if (battle.edit == true)
        {
            WindowModel.Open(WindowEnum.BattleEdit);
        }
        else
        {
            WindowModel.Open(WindowEnum.Battle);
            models.Add(PieceModel.Instance.Init());
        }

        CreateLayer(typeof(TerrainLayer));
        if (battle.edit == false)
        {
            CreateLayer(typeof(PieceLayer));
        }
    }

    public void CreateLayer(Type type)
    {
        WorldModel.CreateLayer(type, layers.Count - 2);
        layers.Add(type);
    }

    public void DeleteLayer(Type type)
    {
        if (layers.Contains(type))
        {
            layers.Remove(type);
            WorldModel.DeleteLayer(type);
        }
    }

    public void Complete()
    {
        StageModel.Instance.Complete();
    }

    public void DestroyBattle()
    {
        if (CameraModel.Instance != null)
        {
            CameraModel.Instance.Target = null;

            foreach (var item in layers)
            {
                WorldModel.DeleteLayer(item);
            }
            layers.Clear();
        }

        for (int i = 0; i < models.Count; i++)
        {
            models[i].Destroy();
        }
        models.Clear();
    }
}