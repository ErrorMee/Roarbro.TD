using System.Collections.Generic;

public class FSM<T>
{
    public HashSet<State<T>> mStates = new HashSet<State<T>>();
    public State<T> mCurrentState;

    public virtual void ChangeState(T info)
    {
        foreach (var item in mStates)
        {
            if (item.mInfo.Equals(info))
            {
                ChangeState(item);
                return;
            }
        }
        mCurrentState = null;
    }

    public virtual void ChangeState(State<T> state)
    {
        if (mCurrentState == state)
        {
            return;
        }

        mCurrentState?.Exit();
        mCurrentState = state;
        mCurrentState?.Enter();
    }

    public virtual void Update()
    {
        mCurrentState?.Update();
    }
}