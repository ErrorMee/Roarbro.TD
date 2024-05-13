using UnityEngine;

[DisallowMultipleComponent]
public abstract class WorldUnit : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    protected virtual void OnEnable()
    {
        LogicUpdateModel.Add(OnLogic);
    }

    protected virtual void OnDisable()
    {
        LogicUpdateModel.Remove(OnLogic);
    }

    protected virtual void OnLogic()
    { }
}
