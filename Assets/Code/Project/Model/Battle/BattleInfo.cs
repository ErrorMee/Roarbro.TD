
public class BattleInfo : ConfigInfo<BattleConfig>
{
    public bool edit = false;

    public BattleInfo(BattleConfig config)
    {
        this.config = config;
    }

}