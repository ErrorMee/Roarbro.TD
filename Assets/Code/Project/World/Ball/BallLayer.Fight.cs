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

                if (unit.info.level > 1 && unit.info.config.id > 0)
                {
                    unit.fsm.ChangeState(BallState.Fight);
                }
            }
        }
    }

    private void FightUpdate()
    {
        
    }
}