using System;

public class State<T>
{
    public T mInfo;

    public Action OnEnter;
    public Action OnExit;
    public Action OnUpdate;

    public State(T info, Action onEnter, Action onExit = null, Action onUpdate = null)
    {
        mInfo = info;
        OnEnter = onEnter;
        OnExit = onExit;
        OnUpdate = onUpdate;
    }

    public virtual void Enter()
    {
        OnEnter?.Invoke();
    }

    public virtual void Exit()
    {
        OnExit?.Invoke();
    }

    public virtual void Update()
    {
        OnUpdate?.Invoke();
    }
}