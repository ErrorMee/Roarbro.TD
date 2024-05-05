
using System;

public class ArmyConfigs : Configs<ArmyConfigs, ArmyConfig>
{
    
}

[Serializable]
public class ArmyConfig : Config
{
    public EnemyInfoConfig[] enemys;

    public EnemyInfoConfig GetEnemyInfoConfig(int posx, int posy)
    {
        return enemys[GridUtil.GetIndex(posx, posy)];
    }
}
