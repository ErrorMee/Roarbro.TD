using System;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyLayer : WorldLayer<EnemyUnit>
{
    private void IdleEnter()
    {
        CreateSelect();
    }

    private void IdleUpdate()
    {
        
    }

}