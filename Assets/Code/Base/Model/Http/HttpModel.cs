using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

public class HttpModel : Singleton<HttpModel>,IDestroy
{
    private const string PostRequestSecret = "e672ce122265468a82b7ff9f";

    private UnityWebRequest postingRequest = null;
    bool showWait = true;
    readonly List<PostKeyValue> postDataKeyValues = new();

    private string lastPostUri = "";

    private Action<bool, string> postCallBack;

    public void Update()
    {
        if (postingRequest != null)
        {
            if (postingRequest.isDone)
            {
                if (showWait)
                {
                    WindowModel.Wait(false);
                }

                if (postingRequest.result == UnityWebRequest.Result.ConnectionError ||
                    postingRequest.result == UnityWebRequest.Result.ProtocolError || postingRequest.responseCode == 0)
                {
                    Debug.LogError(postingRequest.error + " uri:" + 
                        postingRequest.uri + " code " + postingRequest.responseCode);
                    postCallBack(false, null);
                }
                else
                {
                    if (Boot.Config.netLog)
                    {
                        LogUtil.Log("<<<OnPost:", postingRequest.downloadHandler.text, Color.green);
                    }
                    postCallBack(true, postingRequest.downloadHandler.text);
                }
                postingRequest = null;
            }
        }
        
    }

    public static void Post<T>(string uri, T postDataData, 
        Action<bool, string> callBack, bool showWait = true) where T : PostBase
    {
        if (Instance.postingRequest != null)
        {
            Debug.LogError(Instance.lastPostUri + " is still dealing,return " + uri);
            return;
        }

        PostData<T> postData = new()
        {
            channel = LoginModel.Instance.GetChannel()
        };
        postDataData.account = LoginModel.Instance.Account;
        postDataData.token = LoginModel.Instance.GetToken();
        postData.data = postDataData;

        Instance.InitSignData(postDataData);

        string postJson = JsonUtility.ToJson(postData);

        Instance.postCallBack = callBack;

        if (Boot.Config.netLog)
        {
            LogUtil.Log(">>>Post:" + uri + " : " + postJson, Color.green * 0.6f);
        }

        Instance.postingRequest = UnityWebRequest.PostWwwForm(uri, postJson);
        Instance.postingRequest.timeout = 15;
        Instance.lastPostUri = uri;

        Instance.showWait = showWait;
        if (showWait)
        {
            WindowModel.Wait(true);
        }
        Instance.postingRequest.SendWebRequest();
    }

    private void InitSignData(PostBase postDataData)
    {
        postDataKeyValues.Clear();

        Type type = postDataData.GetType();
        FieldInfo[] fieldInfos = type.GetFields();
        for (int i = 0; i < fieldInfos.Length; i++)
        {
            FieldInfo fieldInfo = fieldInfos[i];

            object value = fieldInfo.GetValue(postDataData);

            string stringValue = Convert.ToString(value);

            if (fieldInfo.Name != "sign")
            {
                postDataKeyValues.Add(new PostKeyValue(fieldInfo.Name, stringValue));
            }
        }

        postDataKeyValues.Sort((PostKeyValue a, PostKeyValue b) =>
        {
            return a.key.CompareTo(b.key);
        });

        string signSrc = "";
        for (int i = 0; i < postDataKeyValues.Count; i++)
        {
            PostKeyValue postKeyValue = postDataKeyValues[i];

            signSrc += (postKeyValue.key + "=" + postKeyValue.value);
        }
        signSrc += "secret=" + PostRequestSecret;
        postDataData.sign = MD5Util.StringToMD5Hash(signSrc);
    }
}
