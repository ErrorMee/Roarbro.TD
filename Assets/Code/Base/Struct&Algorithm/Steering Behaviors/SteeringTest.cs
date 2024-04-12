using System;
using System.Collections.Generic;
using UnityEngine;

public class SteeringTest : MonoBehaviour
{
    public SteeredVehicle steeredVehicle;

    private void Update()
    {
        steeredVehicle.Seek(transform.position);
        //steeredVehicle.Wander();
    }
}
