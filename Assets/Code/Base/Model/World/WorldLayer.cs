using UnityEngine;

[DisallowMultipleComponent]
public class WorldLayer<T> : FrameMono where T: MonoBehaviour
{
    protected T unitTemplate;

    protected SelectUnit select;

    protected override void Awake()
    {
        base.Awake();

        unitTemplate = AddressModel.LoadGameObject(Address.UnitPrefab(typeof(T).Name), transform).GetComponent<T>();
        unitTemplate.gameObject.SetActive(false);
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
            select = AddressModel.LoadGameObject(
            Address.UnitPrefab(typeof(SelectUnit).Name), transform).GetComponent<SelectUnit>();
            select.gameObject.SetActive(false);
        }
    }
}
