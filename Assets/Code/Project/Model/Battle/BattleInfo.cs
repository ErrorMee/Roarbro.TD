
public class BattleInfo : ConfigInfo<BattleConfig>
{
    public bool edit = false;

    public TerrainConfig terrain;

    public ArmyConfig army;

    public BattleInfo(BattleConfig config)
    {
        this.config = config;
        terrain = TerrainConfigs.Instance.GetConfigByID(config.terrain);
        army = ArmyConfigs.Instance.GetConfigByID(config.id);
    }

}