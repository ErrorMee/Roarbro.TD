using UnityEngine;

public class BTLeafWait : BTLeaf
{
    private float time;
    public BTLeafWait(float second) : base()
    {
        time = second;
    }

    protected override void OnUpdate()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            Finish();
        }
    }
}