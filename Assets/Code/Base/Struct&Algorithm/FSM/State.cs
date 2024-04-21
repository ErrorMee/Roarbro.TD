using System;

public class State<T>
{
    public T mInfo;

    public Action OnEnter;
    public Action OnExit;
    public Action OnUpdate;

    public State(T info, Action onEnter, Action onUpdate = null, Action onExit = null)
    {
        mInfo = info;
        OnEnter = onEnter;
        OnUpdate = onUpdate;
        OnExit = onExit;
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