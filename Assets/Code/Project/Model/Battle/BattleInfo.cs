
public class BattleInfo : ConfigInfo<BattleConfig>
{
    public bool edit = false;

    public TerrainConfig terrain;

    public ArmyConfig army;

    public float leftHP;
    public float maxHP;

    public BattleInfo(BattleConfig config)
    {
        this.config = config;
        terrain = TerrainConfigs.Instance.GetConfigByID(config.terrain);
        army = ArmyConfigs.Instance.GetConfigByID(config.id);

        leftHP = maxHP = 100;
    }


    public void Attacked(EnemyInfo enemyInfo)
    {
        leftHP -= enemyInfo.leftHP;
        if (leftHP <= 0)
        {
            BattleModel.Instance.pause = true;
            WindowModel.Open(WindowEnum.Lose);
        }
        EventModel.Send(EventEnum.ChangeHP);
    }
}