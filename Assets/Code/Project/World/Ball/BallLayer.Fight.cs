using UnityEngine;

public partial class BallLayer : WorldLayer<BallUnit>
{
    private void FightEnter()
    {
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                BallUnit unit = units[x, y];

                TerrainEnum terrain = BattleModel.Instance.battle.terrain.GetTerrain(unit.info.index.x, unit.info.index.y);
                if (terrain == TerrainEnum.Land && unit.info.config.id > 0)
                //if (unit.info.level > 1 && unit.info.config.id > 0)
                {
                    unit.fsm.ChangeState(BallState.Fight);
                }
                else
                {
                    unit.fsm.ChangeState(BallState.Hide);
                }
            }
        }
    }

    private void FightUpdate()
    {
        
    }
}