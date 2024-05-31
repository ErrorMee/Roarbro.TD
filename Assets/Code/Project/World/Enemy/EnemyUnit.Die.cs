using UnityEngine;

public partial class EnemyUnit : MonoBehaviour
{
    private void DieEnter()
    {
        EventModel.Send(EventEnum.EnemyDie, this);
    }

    private void DieUpdate()
    {
        
    }
}