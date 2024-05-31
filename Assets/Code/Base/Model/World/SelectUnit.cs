
using UnityEngine;

public class SelectUnit : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    void Awake()
    {
        meshRenderer.SetMPBColor(MatPropUtil.BaseColorKey, QualityConfigs.GetColor(QualityEnum.N), false);
    }
}