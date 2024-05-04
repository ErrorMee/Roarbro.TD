using UnityEngine;
using UnityEngine.UI;

public class BattleEditWindow : WindowBase
{
    [SerializeField] SDFBtn terrain;
    [SerializeField] Graphic terrainSelect;
    [SerializeField] SDFBtn enemy;
    [SerializeField] Graphic enemySelect;

    protected override void Awake()
    {
        base.Awake();
        CameraModel.Instance.mainCamera.orthographicSize = 8;

        ClickListener.Add(terrain.transform).onClick += OnClickTerrain;
        ClickListener.Add(enemy.transform).onClick += OnClickEnemy;
    }

    void OnClickTerrain()
    {
        terrainSelect.gameObject.SetActive(true);
        enemySelect.gameObject.SetActive(false);
        Open(WindowEnum.TerrainEdit);
    }

    void OnClickEnemy()
    {
        enemySelect.gameObject.SetActive(true);
        terrainSelect.gameObject.SetActive(false);
        Close(WindowEnum.TerrainEdit);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Close(WindowEnum.TerrainEdit);
        BattleModel.Instance.DestroyBattle();
    }
}