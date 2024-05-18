using System;
using System.Collections.Generic;

public class FSM<T>
{
    private readonly HashSet<State<T>> mStates = new();
    private State<T> mCurrentState;

    public void AddState(T info, Action onEnter, Action onUpdate = null, Action onExit = null)
    {
        mStates.Add(new State<T>(info, onEnter, onUpdate, onExit));
    }

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