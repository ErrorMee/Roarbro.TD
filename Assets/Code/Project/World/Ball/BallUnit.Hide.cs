using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class BallUnit : MonoBehaviour
{
    const float hideSpeed = 0.1f;

    private void HideEnter()
    {
    }

    private void HideUpdate()
    {
        Vector3 scale = transform.localScale;
        scale = new Vector3(scale.x - hideSpeed, scale.y - hideSpeed, scale.z - hideSpeed);
        transform.localScale = scale;
        if (scale.x <= 0)
        {
            transform.localScale = Vector3.zero;
            fsm.ChangeState(BallState.Idle);
        }
    }
}