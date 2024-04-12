using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityObject = UnityEngine.Object;

/// <summary>
/// 负责Address的加载、预加载、卸载; 没有具体的类型; 和业务逻辑无关;
/// </summary>
public partial class AddressModel : Singleton<AddressModel>, IDestroy
{
    class AssetRequestInfo
    {
        public AsyncOperationHandle asyncOperationHandle;
        public string address;
    }

    class AssetLoadedCounter
    {
        public AsyncOperationHandle asyncOperationHandle;
        public int count;
    }

    readonly Dictionary<Action<UnityObject>, List<AssetRequestInfo>> loadingAssets = new();
    readonly Dictionary<string, AssetLoadedCounter> loadedCounters = new();

    readonly Dictionary<string, string> redirectDic = new();

    public AddressModel()
    {
        Addressables.InternalIdTransformFunc = OnRedirect;
    }

    private string OnRedirect(IResourceLocation resourceLocation)
    {
        string redirectURl = resourceLocation.InternalId;
        foreach (var item in redirectDic)
        {
            redirectURl = redirectURl.Replace(item.Key, item.Value);
        }
        return redirectURl;
    }

    /// <summary>
    ///  地址重定向
    /// </summary>
    /// <param name="originalKey"></param>
    /// <param name="redirectKey"></param>
    public void AddRedirect(string originalKey, string redirectKey)
    {
        redirectDic[originalKey] = redirectKey;
    }

    public void ClearRedirect()
    {
        redirectDic.Clear();
    }

    /// <summary>
    /// 异步加载
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="address"></param>
    /// <param name="callBack"></param>
    public void LoadAssetAsync<T>(string address, Action<UnityObject> callBack) where T : UnityObject
    {
        if (!loadingAssets.ContainsKey(callBack))
        {
            loadingAssets.Add(callBack, SharedPool<List<AssetRequestInfo>>.Get());
        }

        List<AssetRequestInfo> list = loadingAssets[callBack];

        bool repeatFlag = false;
        for (int i = 0; i < list.Count; i++)
        {
            AssetRequestInfo requestInfo = list[i];
            if (requestInfo.address == address)
            {
                repeatFlag = true;
                list.RemoveAt(i);
                list.Add(requestInfo);
                break;
            }
        }

        if (!repeatFlag)
        {
            AssetRequestInfo requestInfo = SharedPool<AssetRequestInfo>.Get();
            try
            {
                //LogUtil.Log("↓↓↓LoadAsset:", address,Color.yellow);
                requestInfo.asyncOperationHandle = Addressables.LoadAssetAsync<T>(address);
                requestInfo.address = address;
                if (requestInfo.asyncOperationHandle.IsDone)
                {
                    ProcessIsDoneAssetRequestInfo(requestInfo.asyncOperationHandle, requestInfo.address, callBack);
                    SharedPool<AssetRequestInfo>.Cache(requestInfo);
                }
                else
                {
                    list.Add(requestInfo);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    /// <summary>
    /// 同步加载
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="address"></param>
    /// <returns></returns>
    public T LoadAsset<T>(string address) where T : UnityObject
    {
        AsyncOperationHandle asyncOperationHandle = Addressables.LoadAssetAsync<T>(address);
        asyncOperationHandle.WaitForCompletion();
        ProcessIsDoneAssetRequestInfo(asyncOperationHandle, address, null);
        return (T)asyncOperationHandle.Result;
    }

    private bool HasAddress(string address)
    {
        AsyncOperationHandle<IList<IResourceLocation>> asyncOperationHandle =
            Addressables.LoadResourceLocationsAsync(address);
        asyncOperationHandle.WaitForCompletion();

        if (asyncOperationHandle.Result != null && asyncOperationHandle.Result.Count > 0)
        {
            return true;
        }
        return false;
    }

    readonly List<Action<UnityObject>> removeKeys = new();
    readonly List<Action<UnityObject>> loadingKeys = new();
    public void Update()
    {
        if (loadingAssets.Count == 0) return;
        removeKeys.Clear();
        loadingKeys.Clear();

        foreach (var item in loadingAssets)
        {
            loadingKeys.Add(item.Key);
        }

        for (int i = 0; i < loadingKeys.Count; i++)
        {
            Action<UnityObject> key = loadingKeys[i];
            List<AssetRequestInfo> value = loadingAssets[key];

            int len = value.Count;
            for (int j = len - 1; j >= 0; j--)
            {
                AssetRequestInfo requestInfo = value[j];
                if (requestInfo.asyncOperationHandle.IsDone)
                {
                    ProcessIsDoneAssetRequestInfo(requestInfo.asyncOperationHandle, requestInfo.address, key);
                    SharedPool<AssetRequestInfo>.Cache(requestInfo);
                    len--;

                    value.Remove(requestInfo);
                }
                else break;
            }
            if (0 == len) removeKeys.Add(key);
        }

        for (int i = 0; i < removeKeys.Count; i++)
        {
            List<AssetRequestInfo> removeRequests = loadingAssets[removeKeys[i]];
            removeRequests.Clear();
            SharedPool<List<AssetRequestInfo>>.Cache(removeRequests);
            loadingAssets.Remove(removeKeys[i]);
        }
    }

    /// <summary>
    /// 卸载
    /// </summary>
    /// <param name="address"></param>
    /// <param name="unloadCount">卸载次数，0是卸载所有</param>
    /// <returns></returns>
    public bool UnloadAsset(string address, int unloadCount = 1)
    {
        if (loadedCounters.TryGetValue(address, out AssetLoadedCounter loadedCounter))
        {
            if (unloadCount > 0)
            {
                for (int i = loadedCounter.count - 1; i >= 0; i--)
                {
                    if (unloadCount > 0)
                    {
                        //LogUtil.Log("---Release:", address, Color.green);
                        Addressables.Release(loadedCounter.asyncOperationHandle);
                        loadedCounter.count--; unloadCount--;
                        if (loadedCounter.count <= 0)
                        {
                            UnLoadComplete(address, loadedCounter);
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                for (int i = loadedCounter.count - 1; i >= 0; i--)
                {
                    //LogUtil.Log("---Release:", address, Color.green);
                    Addressables.Release(loadedCounter.asyncOperationHandle);
                }
                UnLoadComplete(address, loadedCounter);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UnLoadComplete(string address, AssetLoadedCounter loadedCount)
    {
        loadedCount.count = 0;
        SharedPool<AssetLoadedCounter>.Cache(loadedCount);
        loadedCounters.Remove(address);
    }

    private void ProcessIsDoneAssetRequestInfo(AsyncOperationHandle asyncOperationHandle, string address, Action<UnityObject> callBack)
    {
        if (asyncOperationHandle.Result == null)
        {
            string nullMsg = "未发现:" + address.Replace("Assets/ProductAssets/", "");
            Debug.LogError("↓↓↓LoadAsset:" + nullMsg + " Try <RefreshAsset>");
        }

        try
        {
            callBack?.Invoke((UnityObject)asyncOperationHandle.Result);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        if (!loadedCounters.TryGetValue(address, out AssetLoadedCounter loadedCounter))
        {
            loadedCounter = SharedPool<AssetLoadedCounter>.Get();
            loadedCounter.asyncOperationHandle = asyncOperationHandle;
            loadedCounters.Add(address, loadedCounter);
        }
        loadedCounter.count++;
    }
}