using System.Collections.Generic;

public class FSM<T>
{
    public HashSet<State<T>> mStates = new HashSet<State<T>>();
    public State<T> mCurrentState;

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