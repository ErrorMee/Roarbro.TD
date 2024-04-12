using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AddressableConfig", menuName = "Addressables/Config")]
public class AddressableConfig : ScriptableObject
{
    public string rootFolder = "Assets/Art";

    public string[] stripFolder;
}

[CustomEditor(typeof(AddressableConfig))]
public class AddressableConfigEditor : Editor
{
    AddressableConfig Config => target as AddressableConfig;

    Vector2 stripScrollPos = Vector2.zero;

    private void Save()
    {
        EditorUtility.SetDirty(Config);
        AssetDatabase.SaveAssetIfDirty(Config);
    }

    public override void OnInspectorGUI()
    {
        if (string.IsNullOrEmpty(Config.rootFolder))
        {
            GUI.color = Color.red;
        }
        else
        {
            GUI.color = Color.green;
        }

        string lable = "Root Folder :  " + (string.IsNullOrEmpty(Config.rootFolder) ? "null" : Config.rootFolder);
        if (GUILayout.Button(lable, GUILayout.Width(256)))
        {
            string path = EditorUtility.OpenFolderPanel("Pick Root Folder", Application.dataPath, "Art");

            if (!string.IsNullOrEmpty(path))
            {
                if (path.Contains(Application.dataPath + "/"))
                {
                    Config.rootFolder = path.Replace("\\", "/").Replace(Application.dataPath + "/", string.Empty);
                }
                else
                {
                    Config.rootFolder = null;
                }
            }
            //EditorGUIUtility.ShowObjectPicker<GameObject>(null, false, "", 0);
            //Debug.Log("Picker " + EditorGUIUtility.GetObjectPickerObject());
            Save();
        }
        
        GUI.color = Color.white;

        if (Config.stripFolder == null)
        {
            Config.stripFolder = new string[0];
        }

        if (GUILayout.Button("Add Strip Folder", GUILayout.Width(128)))
        {
            string[] stripNew = new string[Config.stripFolder.Length + 1];
            Config.stripFolder.CopyTo(stripNew, 0);
            Config.stripFolder = stripNew;
            Save();
        }

        stripScrollPos = EditorGUILayout.BeginScrollView(stripScrollPos, GUILayout.Width(360), GUILayout.Height(240));

        for (int i = 0; i < Config.stripFolder.Length; i++)
        {
            string strip = Config.stripFolder[i];

            if (string.IsNullOrEmpty(strip))
            {
                GUI.color = Color.yellow;
                lable = "Pick Strip";
            }
            else
            {
                if (strip.IndexOf(Config.rootFolder) == 0)
                {
                    GUI.color = Color.white;
                }
                else
                {
                    GUI.color = Color.red;
                }
                lable = "Strip : " + strip;
            }

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("-", GUILayout.Width(32)))
            {
                string[] stripNew = new string[Config.stripFolder.Length - 1];

                Array.Copy(Config.stripFolder, 0, stripNew, 0, i);
                Array.Copy(Config.stripFolder, i + 1, stripNew, i, Config.stripFolder.Length - i - 1);

                Config.stripFolder = stripNew;
                Save();
            }
            
            if (GUILayout.Button(lable, GUILayout.Width(300)))
            {
                string path = EditorUtility.OpenFolderPanel("Pick Strip", Application.dataPath, "Art");

                if (!string.IsNullOrEmpty(path))
                {
                    if (path.Contains(Application.dataPath + "/"))
                    {
                        Config.stripFolder[i] = path.Replace("\\", "/").Replace(Application.dataPath + "/", string.Empty);
                    }
                    else
                    {
                        Config.stripFolder[i] = null;
                    }
                }
                Save();
            }

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    }
}