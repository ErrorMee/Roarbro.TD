using System;
/// <summary>
/// 行为树基类
/// </summary>
abstract public class BTBase
{
    public BTStatus Status
    {
        get;
        set;
    } = BTStatus.Wait;

    public Func<bool> check;

    public BTBase()  { }

    /// <summary>
    /// 执行
    /// </summary>
    virtual public void Execute()
    {
        switch (Status)
        {
            case BTStatus.Wait:
                if (OnCheck())
                {
                    Status = BTStatus.Start;
                    OnExecute();
                }
                break;
            case BTStatus.Start:
                if (OnCheck())
                {
                    Status = BTStatus.Update;
                    OnExecute();
                }
                else
                {
                    Finish();
                }
                break;
            case BTStatus.Update:
                if (OnCheck())
                {
                    OnExecute();
                }
                else
                {
                    Finish();
                }
                break;
            case BTStatus.Finish:
                OnExecute();
                break;
        }
    }

    bool OnCheck() 
    {
        if (check == null)
        {
            return true;
        }
        return check.Invoke(); 
    }

    /// <summary>
    /// 被执行 Start [Update] Finish 都会进
    /// </summary>
    virtual protected void OnExecute() { }

    /// <summary>
    /// 结束
    /// </summary>
    public void Finish()
    {
        if (Status != BTStatus.Wait && Status != BTStatus.Finish)
        {
            Status = BTStatus.Finish;
            OnExecute();
        }
    }

    public virtual void Clear()
    {
        Status = BTStatus.Wait;
        check = null;
    }
}