using UnityEngine;

public class BTLeafLog : BTLeaf
{
    readonly string log = "";
    readonly bool oneTime;
    public BTLeafLog(string data, bool oneTime = true) : base()
    {
        log = data;
        this.oneTime = oneTime;
    }

    protected override void OnStart()
    {
        Debug.Log("OnStart:     " + log);
        if (oneTime)
        {
            Finish();
        }
    }

    protected override void OnUpdate()
    {
        Debug.Log("OnUpdate:    " + log);
    }

    protected override void OnFinish()
    {
        Debug.Log("OnFinish:    " + log);
    }
}