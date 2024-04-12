using UnityEngine;

[DisallowMultipleComponent]
public abstract class BattleUnit : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    protected virtual void OnEnable()
    {
        BattleTimerModel.Add(OnLogic, TimerEnum.Logic);
    }

    protected virtual void OnDisable()
    {
        BattleTimerModel.Remove(OnLogic, TimerEnum.Logic);
    }

    protected virtual void OnLogic()
    { }
}
