
public class BTLeaf : BTBase
{
    override protected void OnExecute()
    {
        switch (Status)
        {
            case BTStatus.Start:
                OnStart();
                break;
            case BTStatus.Update:
                OnUpdate();
                break;
            case BTStatus.Finish:
                OnFinish();
                break;
        }
    }

    virtual protected void OnStart() { }
    virtual protected void OnUpdate() { }
    virtual protected void OnFinish() { }
}