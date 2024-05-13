using UnityEngine;

public partial class EnemyUnit : WorldUnit
{
    private void DieEnter()
    {
        EventModel.Send(EventEnum.EnemyDie, this);
    }

    private void DieUpdate()
    {
        
    }
}