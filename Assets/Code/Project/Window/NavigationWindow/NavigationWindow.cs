using UnityEngine;

public class NavigationWindow : WindowBase
{
    [SerializeField] SDFBtn backBtn;
    [SerializeField] SDFBtn homeBtn;

    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);
        ClickListener.Add(backBtn.transform).onClick = OnClickBack;
        ClickListener.Add(homeBtn.transform).onClick = OnClickHome;

        WindowLayer windowLayer = WindowModel.GetLayer(WindowLayerEnum.Bot);
        bool hasBattle = WindowModel.Has(WindowEnum.Battle);
        homeBtn.gameObject.SetActive(windowLayer.GetWindows().Count > 2  && !hasBattle);
    }

    void OnClickBack()
    {
        WindowLayer windowLayer = WindowModel.GetLayer(WindowLayerEnum.Bot);
        WindowBase window = windowLayer.GetLastWindow();
        WindowModel.Close(window.config.id);
    }

    void OnClickHome()
    {
        WindowLayer windowLayer = WindowModel.GetLayer(WindowLayerEnum.Bot);
        WindowBase window = windowLayer.GetLastWindow();
        while (window.config.id != (int)WindowEnum.Lobby)
        {
            WindowModel.Close(window.config.id);
            window = windowLayer.GetLastWindow();
        }
    }
}