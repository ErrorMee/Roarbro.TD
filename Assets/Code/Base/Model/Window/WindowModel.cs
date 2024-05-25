using System;
using System.Collections.Generic;
using UnityEngine;

public partial class WindowModel : SingletonBehaviour<WindowModel>
{
	int UILayerID = 0;
    readonly Dictionary<WindowLayerEnum, WindowLayer> layers = new();
    readonly HashSet<int> loadingWindows = new();

    public void Init()
	{
		UILayerID = LayerMask.NameToLayer("UI");

		InitLayer();
		AirModel.Instance.Init();
        OpenWindow((int)WindowEnum.Splash);
    }

    private void InitLayer()
	{
        foreach (byte value in Enum.GetValues(typeof(WindowLayerEnum)))
        {
            int sortOrder = 1000 * value;
			WindowLayerEnum windowLayerEnum = (WindowLayerEnum)value;
            GameObject windowLayerObj = new();
            windowLayerObj.transform.SetParent(transform, false);
            WindowLayer layer = windowLayerObj.GetOrAddComponent<WindowLayer>();
            layer.Init(windowLayerEnum, sortOrder);
            layers.Add(windowLayerEnum, layer);
        }
	}

	private void LoadWindow(WindowConfig config, object obj)
	{
		loadingWindows.Add(config.id);

		WindowLayer layer = layers[config.layer];

        AddressModel.LoadGameObjectAsync(config.Address, layer.transform, ((gameObj) =>
		{
			gameObj.layer = UILayerID;
			WindowBase window = gameObj.GetOrAddComponent<WindowBase>();

			if (loadingWindows.Contains(config.id))
			{
				loadingWindows.Remove(config.id);
			}
			else
			{
				window.gameObject.SetActive(false);//保证gameObject上的OnDisable都调用一遍
				Destroy(window.gameObject);
				return;
			}

			window.config = config;
			if (config.layer == WindowLayerEnum.Bot)
			{
				LinkedList<WindowBase> allWindows = layer.GetWindows();
				foreach (WindowBase windowPre in allWindows)
				{
					if (windowPre.isActiveAndEnabled)
					{
						windowPre.gameObject.SetActive(false);//前面界面的disable先调用
					}
				}
			}
			layer.AddWindow(window);
            window.OnOpen(obj);
		}));
	}

	private void OnCloseWindow(WindowLayer layer, WindowBase window)
	{
        layer.RemoveWindow(window);
		window.gameObject.SetActive(false);//关闭界面的disable先调用
		AddressModel.Instance.UnloadAsset(window.config.Address);
		Destroy(window.gameObject);
	}

    private WindowBase GetWindow(int id)
	{
		WindowConfig config = WindowConfigs.ConfigByID(id);
		WindowLayer layer = layers[config.layer];
		WindowBase windowBase = layer.GetWindow(id);
		return windowBase;
	}

    private bool IsLoading(int id)
    {
        return loadingWindows.Contains(id);
    }

    private bool HasWindow(int window)
    {
        return IsLoading(window) || GetWindow(window) != null;
    }

    private WindowLayer GetWindowLayer(WindowLayerEnum windowLayer)
    {
        return layers[windowLayer];
    }

    private void OpenWindow(int id, object obj = null)
    {
        if (Instance.loadingWindows.Contains(id))
        {
            Debug.LogWarning("所有界面单例 " + id);
            return;
        }

        WindowConfig config = WindowConfigs.ConfigByID(id);

        if (config != null)
        {
            WindowBase window = Instance.GetWindow(id);

            if (window == null)
            {
                Instance.LoadWindow(config, obj);
            }
            else
            {
                window.OnOpen(obj);
            }
        }
        else
        {
            Debug.LogError(id.ToString() + " not find !!! <WindowConfigs>");
        }
    }

    private void CloseWindow(int id)
    {
        if (Instance.loadingWindows.Contains(id))
        {
            Instance.loadingWindows.Remove(id);
            return;
        }

        WindowConfig config = WindowConfigs.ConfigByID(id);

        WindowLayer layer = Instance.layers[config.layer];
        WindowBase window = layer.GetWindow(id);
        if (window == null) return;

        bool needOpenPre = false;

        if (config.layer == WindowLayerEnum.Bot && layer.GetLastWindow().config.id == id)
        {
            WindowBase windowPre = layer.GetLastWindow(1);
            while (windowPre != null)
            {
                if (windowPre.config.id == id)
                {
                    Instance.OnCloseWindow(layer, windowPre);
                    windowPre = layer.GetLastWindow(1);
                }
                else break;
            }

            if (windowPre != null)
            {
                if (!windowPre.isActiveAndEnabled)
                {
                    needOpenPre = true;
                    Instance.OnCloseWindow(layer, window);
                    // 打开界面的enable后调用
                    windowPre.OnOpen(null);
                }
            }
        }

        if (!needOpenPre)
        {
            Instance.OnCloseWindow(layer, window);
        }
    }
}