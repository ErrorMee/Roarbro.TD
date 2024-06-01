using UnityEngine;

[DisallowMultipleComponent]
public class WorldLayer<T> : FrameMono where T: MonoBehaviour
{
    protected T unitTemplate;

    protected SelectUnit select;

    protected override void Awake()
    {
        base.Awake();
        string prefabName = typeof(T).Name;
        unitTemplate = AddressModel.LoadGameObject(Address.UnitPrefab(prefabName), transform).GetComponent<T>();
        unitTemplate.gameObject.SetActive(false);
        unitTemplate.name = prefabName;
    }

    protected T CreateUnit(bool active = true)
    {
        T unit = Instantiate(unitTemplate, transform);
        unit.gameObject.SetActive(active);
        return unit;
    }

    protected void CreateSelect()
    {
        if (select == null)
        {
            string prefabName = typeof(SelectUnit).Name;
            select = AddressModel.LoadGameObject(
            Address.UnitPrefab(prefabName), transform).GetComponent<SelectUnit>();
            select.gameObject.SetActive(false);
        }
    }
}
