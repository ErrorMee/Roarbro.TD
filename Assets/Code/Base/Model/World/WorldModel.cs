using System;
using System.Collections.Generic;
using UnityEngine;

public partial class WorldModel : SingletonBehaviour<WorldModel>
{
    private Dictionary<Type, Transform> layers = new Dictionary<Type, Transform>();

}