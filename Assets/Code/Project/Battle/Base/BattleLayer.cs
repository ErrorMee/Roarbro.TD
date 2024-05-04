using UnityEngine;

[DisallowMultipleComponent]
public class BattleLayer<T> : FrameMono where T: BattleUnit
{
    protected T unitTemplate;

    protected override void Awake()
    {
        base.Awake();

        unitTemplate = AddressModel.LoadGameObject(Address.UnitPrefab(typeof(T).Name), transform).GetComponent<T>();
        unitTemplate.gameObject.SetActive(false);
    }

    public T CreateUnit(bool active = true)
    {
        T unit = Instantiate(unitTemplate.gameObject, transform).GetComponent<T>();
        unit.gameObject.SetActive(active);
        return unit;
    }
}