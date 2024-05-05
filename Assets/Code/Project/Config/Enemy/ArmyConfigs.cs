
using System;

public class ArmyConfigs : Configs<ArmyConfigs, ArmyConfig>
{
    
}

[Serializable]
public class ArmyConfig : Config
{
    public ArmyEnemyConfig[] enemys;
}
