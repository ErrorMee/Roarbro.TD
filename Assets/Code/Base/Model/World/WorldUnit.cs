using UnityEngine;

[DisallowMultipleComponent]
public abstract class WorldUnit : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    protected virtual void OnEnable()
    {
    }

    protected virtual void OnDisable()
    {
    }

    protected virtual void FixedUpdate()
    { 
    }
}
