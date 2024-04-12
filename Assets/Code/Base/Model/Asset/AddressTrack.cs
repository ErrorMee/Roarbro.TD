
using UnityEngine;

[DisallowMultipleComponent]
public class AddressTrack : MonoBehaviour
{
    [SerializeField]
    [ReadOnlyProperty]
    string address;
	int markedInstanceId;

	public void Address(string address)
	{
		this.address = address;
        //只对第一个实例做unload，后续clone的不做
        markedInstanceId = gameObject.GetInstanceID();
    }

    private void OnDestroy()
    {
        if (markedInstanceId == gameObject.GetInstanceID())
        {
            AddressModel.Instance.UnloadAsset(address);
        }
    }
}
