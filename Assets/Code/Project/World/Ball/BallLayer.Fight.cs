using UnityEngine;

public partial class BallLayer : WorldLayer<BallUnit>
{
    private void FightEnter()
    {
        BallModel.Instance.fighting = true;
    }

    private void FightUpdate()
    {
        
    }
}