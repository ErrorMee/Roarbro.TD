using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CreateWindow : EditorWindow
{
    //[MenuItem(BootConfig.company + "/CreateWindow")]
    public static void OpenCreateWindow()
    {
        GetWindow(typeof(CreateWindow), true, "CreateWindow").Show();
    }

    string typeName = "?";
    string windowName = "?Window";
    public GameObject prefab;
    string prefabPath;
    WindowLayerEnum layer = WindowLayerEnum.Mid;
    WindowShowEnum show = WindowShowEnum.None;

    void OnGUI()
    {
        GUI.color = Color.white;
        Type type = Assembly.Load("Assembly-CSharp").GetType(windowName);
        if (type != null && prefab != null && prefab.GetComponent(type) == null)
        {
            prefab.AddComponent(type);
            PrefabUtility.SaveAsPrefabAssetAndConnect(prefab, prefabPath, InteractionMode.AutomatedAction);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            Close();
            DestroyImmediate(prefab);

            AddWindowConfigs();

            ManagedAddressable.FastRefreshGroup();
        }

        GUILayout.Label("WindowName"); 
        typeName = EditorGUILayout.TextField(typeName, GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.Label("WindowLayerEnum");
        layer = (WindowLayerEnum)EditorGUILayout.EnumPopup(layer, GUILayout.Width(60));

        GUILayout.Label("WindowShowEnum");
        show = (WindowShowEnum)EditorGUILayout.EnumPopup(show, GUILayout.Width(60));

        GUILayout.Space(8);
        windowName = typeName + "Window";
        GUILayout.Label($"1.Create:Code/Project/Window/{windowName}/{windowName}.cs");
        GUILayout.Label($"2.Create:Art/Window/{windowName}/{windowName}.prefab");
        GUILayout.Label($"3.Code/Base/Config/Window/WindowEnum.cs AddEnum {typeName}");
        GUILayout.Label($"4.Add Art/Config/WindowConfigs.asset");

        GUILayout.Space(8);

        if (type != null)
        {
            GUI.color = Color.red;
            GUILayout.Label("Type already exists");
        }
        else
        {
            if (!Regex.IsMatch(typeName, RegexUtil.Partern4Az13))
            {
                GUI.color = Color.red;
                GUILayout.Label(typeName + " is illegal name");
            }
            else
            {
                if (GUILayout.Button("Create", GUILayout.Width(80)))
                {
                    CreateCode();
                    CreatePrefab();
                    AddEnum();
                }
            }
        }
    }

    void CreateCode()
    {
        Directory.CreateDirectory(Application.dataPath + "/Code/Project/Window/" + windowName);

        StreamReader streamReader = new(Application.dataPath + "/Editor/Base/ScriptTemplates/Window.cs.txt");
        string template = streamReader.ReadToEnd();
        template = template.Replace("#SCRIPTNAME#", windowName);
        streamReader.Close();
        streamReader.Dispose();

        string codeFilePath = Application.dataPath + "/Code/Project/Window/" + windowName + "/" + windowName + ".cs";
        StreamWriter streamWriter = new(codeFilePath, false, new UTF8Encoding(false));
        streamWriter.Write(template);
        streamWriter.Close();
        streamWriter.Dispose();
    }
    void CreatePrefab()
    {
        prefabPath = $"Assets/Art/Window/{windowName}";
        Directory.CreateDirectory(prefabPath);
        prefabPath = $"Assets/Art/Window/{windowName}/{windowName}.prefab";
        prefab = GameObject.Find(windowName);
        if (prefab == null)
        {
            prefab = new(windowName);
            prefab.transform.SetParent(GameObject.Find("Window").transform);
            prefab.transform.localPosition = Vector3.zero;
            prefab.transform.localScale = Vector3.one;
            prefab.transform.localRotation = Quaternion.Euler(Vector3.zero);
            RectTransform rect = prefab.GetOrAddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.gameObject.layer = LayerMask.NameToLayer("UI");

            GameObject bg = new("BG");
            bg.transform.SetParent(prefab.transform);
            bg.transform.localPosition = Vector3.zero;
            bg.transform.localScale = Vector3.one;
            bg.transform.localRotation = Quaternion.Euler(Vector3.zero);
            rect = bg.GetOrAddComponent<RectTransform>();
            bg.GetOrAddComponent<Image>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.gameObject.layer = LayerMask.NameToLayer("UI");
        }
        PrefabUtility.SaveAsPrefabAssetAndConnect(prefab, prefabPath, InteractionMode.AutomatedAction);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    void AddEnum()
    {
        string enumPath = Application.dataPath + "/Code/Base/Config/Window/WindowEnum.cs";
        StreamReader streamReader = new(enumPath);
        string template = streamReader.ReadToEnd();
        string[] dataSplit = template.Split('}');
        streamReader.Close();
        streamReader.Dispose();

        StreamWriter streamWriter = new(enumPath, false, new UTF8Encoding(false));
        streamWriter.Write(dataSplit[0]);
        streamWriter.WriteLine($"    {typeName},");
        streamWriter.Write('}');
        streamWriter.Close();
        streamWriter.Dispose();
    }

    void AddWindowConfigs()
    {
        WindowConfigs configs = AssetDatabase.LoadAssetAtPath<WindowConfigs>("Assets/Art/Config/WindowConfigs.asset");

        WindowConfig[] allNew = new WindowConfig[configs.all.Length + 1];
        configs.all.CopyTo(allNew, 0);
        WindowConfig newConfig = new();
        allNew[^1] = newConfig;

        int maxV = 0;
        foreach (WindowEnum value in Enum.GetValues(typeof(WindowEnum)))
        {
            maxV = Mathf.Max(maxV, (int)value);
        }

        newConfig.id = maxV;
        Debug.LogError("id " + newConfig.id);

        newConfig.layer = layer;
        newConfig.show = show;
        configs.all = allNew;

        EditorUtility.SetDirty(configs);
        AssetDatabase.SaveAssetIfDirty(configs);
    }
}
