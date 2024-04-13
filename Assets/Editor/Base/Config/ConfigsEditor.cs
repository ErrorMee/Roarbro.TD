using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConfigsEditor<C, S> : Editor where C: Config, new() where S : Configs<S, C>
{
    protected S Configs => target as S;

    bool sortUp = true;

    public const string ConfigMenu = "Assets/" + BootConfig.company + "/Configs/";
    const string assetPath = "Assets/Art/Config/{0}Configs.asset";

    int lastID = 0;

    LanguageConfigs languageConfigs;
    protected int[] languageIDs;
    protected string[] languageStrs;
    protected Vector2Int languageRange = Vector2Int.zero;

    GUIStyle lableStyle;

    public static void CreateConfigs<T>(string name) where T: ConfigsBase
    {
        AssetDatabase.CreateAsset(CreateInstance<T>(), string.Format(assetPath, name));
    }

    public override void OnInspectorGUI()
    {
        C config;

        lableStyle ??= new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter
            };
        
        EditorGUILayout.BeginHorizontal();
        GUI.color = Color.cyan;
        DrawMenu();
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();

        GUI.color = Color.magenta;
        for (int h = 0; h < Configs.pageColumn; h += 1)
        {
            DrawHead();
        }
        GUI.color = Color.white;
        
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();

        int pageCapacity = Configs.GetPageCapacity();
        int startIndex = Mathf.Max((Configs.pageCrt - 1) * pageCapacity, 0);
        int endIndex = Mathf.Min(Configs.pageCrt * pageCapacity, Configs.all == null ? 0 : Configs.all.Length);
        
        EditorGUILayout.BeginVertical();
        for (int x = startIndex; x < endIndex; x += Configs.pageColumn)
        {
            EditorGUILayout.BeginHorizontal();

            for (int c = 0; c < Configs.pageColumn; c += 1)
            {
                int index = x + c;

                if (!sortUp)
                {
                    index = Configs.all.Length - index - 1;
                }

                if (index >= Configs.all.Length)
                {
                    break;
                }
                config = Configs.all[index];
                config ??= Configs.all[index] = new C();
                bool odd = index % 2 == 1;
                GUI.color = odd ? Color.white : Color.white * 0.8f;

                DrawConfig(index, config);

                lastID = config.id;
            }

            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("", GUILayout.Width(16));
        }

        EditorGUILayout.BeginHorizontal(GUILayout.Width(120));
        if (GUILayout.Button("Save", GUILayout.Width(40)))
        {
            EditorUtility.SetDirty(Configs);
            AssetDatabase.SaveAssetIfDirty(Configs);

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }
        Configs.all = ArrayOeprate(Configs.all, null, true);
        DrawMenuAdd();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    void DrawMenu()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save", GUILayout.Width(40)))
        {
            EditorUtility.SetDirty(Configs);
            AssetDatabase.SaveAssetIfDirty(Configs);

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }

        Configs.all = ArrayOeprate(Configs.all, null, true);

        EditorGUILayout.LabelField(" -", GUILayout.Width(15));
        Configs.showDel = EditorGUILayout.Toggle(Configs.showDel, GUILayout.Width(15));

        EditorGUILayout.LabelField(" +", GUILayout.Width(15));
        Configs.showAdd = EditorGUILayout.Toggle(Configs.showAdd, GUILayout.Width(15));

        EditorGUILayout.LabelField(" Use", GUILayout.Width(28));
        Configs.showUse = EditorGUILayout.Toggle(Configs.showUse, GUILayout.Width(15));

        EditorGUILayout.LabelField(" Idx", GUILayout.Width(22));
        Configs.showIdx = EditorGUILayout.Toggle(Configs.showIdx, GUILayout.Width(15));

        EditorGUILayout.LabelField(" Id", GUILayout.Width(15));
        Configs.showId = EditorGUILayout.Toggle(Configs.showId, GUILayout.Width(15));

        EditorGUILayout.LabelField(" Name", GUILayout.Width(40));
        Configs.showName = EditorGUILayout.Toggle(Configs.showName, GUILayout.Width(40));

        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        DrawMenuAdd();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    protected virtual void DrawMenuAdd()
    {
        EditorGUILayout.LabelField("⇉", GUILayout.Width(15));
        Configs.pageRow = EditorGUILayout.IntField(Configs.pageRow, GUILayout.Width(20));
        Configs.pageRow = Mathf.Clamp(Configs.pageRow, 1, 20);

        EditorGUILayout.LabelField("⇊", GUILayout.Width(15));
        Configs.pageColumn = EditorGUILayout.IntField(Configs.pageColumn, GUILayout.Width(20));
        Configs.pageColumn = Mathf.Clamp(Configs.pageColumn, 1, 8);

        int pageMax = 1;
        if (Configs.all != null)
        {
            pageMax = Mathf.CeilToInt((Configs.all.Length + 0f) / (Configs.GetPageCapacity()));
        }

        if (GUILayout.Button("<", GUILayout.Width(18)))
        {
            Configs.pageCrt--;
            if (Configs.pageCrt <= 0)
            {
                Configs.pageCrt = pageMax;
            }
        }
        DrawLabel(Configs.pageCrt + "/" + pageMax, 40);

        if (GUILayout.Button(">", GUILayout.Width(18)))
        {
            Configs.pageCrt++;
            if (Configs.pageCrt > pageMax)
            {
                Configs.pageCrt = 1;
            }
        }

        if (Configs.all != null)
        {
            DrawLabel("All " + Configs.all.Length, 50);
        }
    }

    protected void GotoConfigs(string label, int width)
    {
        if (GUILayout.Button(label, GUILayout.Width(width)))
        {
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(string.Format(assetPath, label));
        }
    }

    protected ConfigsBase GetConfigs(string label)
    {
        return AssetDatabase.LoadAssetAtPath<ConfigsBase>(string.Format(assetPath, label));
    }

    protected virtual void DrawHead()
    {
        if (languageConfigs == null)
        {
            if (Configs is LanguageConfigs)
            {
                languageConfigs = Configs as LanguageConfigs;
            }
            else
            {
                languageConfigs = GetConfigs("Language") as LanguageConfigs;
            }

            List<LanguageConfig> rangeLanguageConfigs = new();
            for (int i = 0; i < languageConfigs.all.Length; i++)
            {
                LanguageConfig config = languageConfigs.all[i];
                if (config.id > languageRange.x && config.id <= languageRange.y)
                {
                    rangeLanguageConfigs.Add(config);
                }
            }

            languageIDs = new int[rangeLanguageConfigs.Count];
            languageStrs = new string[rangeLanguageConfigs.Count];
            for (int i = 0; i < rangeLanguageConfigs.Count; i++)
            {
                LanguageConfig config = rangeLanguageConfigs[i];
                languageIDs[i] = config.id;
                languageStrs[i] = config.values[0];
            }
        }

        if (Configs.showDel)
        {
            EditorGUILayout.LabelField(" - ", GUILayout.Width(20));
        }

        if (Configs.showAdd)
        {
            EditorGUILayout.LabelField(" + ", GUILayout.Width(20));
        }

        if (Configs.showUse)
        {
            EditorGUILayout.LabelField("", GUILayout.Width(20));
        }

        if (Configs.showIdx)
        {
            EditorGUILayout.LabelField("Idx", GUILayout.Width(30));
        }

        if (Configs.showId)
        {
            EditorGUILayout.LabelField("Id", GUILayout.Width(50));
        }

        if (Configs.showName)
        {
            EditorGUILayout.LabelField("Name", GUILayout.Width(65));
        }
    }

    protected void DrawLabel(string label, int width)
    {
        EditorGUILayout.LabelField(label, lableStyle, GUILayout.Width(width));
    }

    protected virtual void DrawConfig(int index, C config)
    {
        if (Configs.showDel)
        {
            Configs.all = ArrayDelete(Configs.all, index);
        }

        if (Configs.showAdd)
        {
            Configs.all = ArrayAdd(Configs.all, index);
        }

        if (Configs.showUse)
        {
            config.use = EditorGUILayout.Toggle(config.use, GUILayout.Width(20));
        }

        if (Configs.showIdx)
        {
            EditorGUILayout.LabelField(index.ToString(), GUILayout.Width(30));
        }

        if (Configs.showId)
        {
            config.id = EditorGUILayout.IntField(config.id, GUILayout.Width(50));
        }

        if (Configs.showName)
        {
            config.name = EditorGUILayout.IntPopup(config.name, languageStrs, languageIDs, GUILayout.Width(65));
        }
        if (config.id < 0)
        {
            config.id = lastID + 1;
        }
    }

    protected virtual int ID(int index)
    {
        return 1000 + index;
    }

    protected T[] ArrayDelete<T>(T[] array, int index)
    {
        if (GUILayout.Button("-", GUILayout.Width(20)))
        {
            T[] allNew = new T[array.Length - 1];
            Array.Copy(array, 0, allNew, 0, index);
            Array.Copy(array, index + 1, allNew, index, array.Length - index - 1);
            return allNew;
        }
        return array;
    }

    protected T[] ArrayAdd<T>(T[] array, int index)
    {
        if (GUILayout.Button("+", GUILayout.Width(20)))
        {
            T[] allNew = new T[array.Length + 1];
            Array.Copy(array, 0, allNew, 0, index + 1);
            Array.Copy(array, index + 1, allNew, index + 2, array.Length - index - 1);
            return allNew;
        }
        return array;
    }

    protected T[] ArrayOeprate<T>(T[] array, Config config, 
        bool needSort = false, int width = 30, int capacity = 999)
    {
        EditorGUILayout.BeginHorizontal(GUILayout.Width(width + 42));
        if (array == null && GUILayout.Button("+", GUILayout.Width(width)))
        {
            array = new T[1];
            return array;
        }

        if (array != null && capacity > array.Length && GUILayout.Button("+", GUILayout.Width(width)))
        {
            T[] allNew = new T[array.Length + 1];
            array.CopyTo(allNew, 0);
            return allNew;
        }

        if (array != null)
        {
            if (array.Length > 1 && needSort && 
                GUILayout.Button("Sort" + ((config == null ? sortUp : config.sortUp) ? "↑" : "↓"), GUILayout.Width(42)))
            {
                if (config == null)
                {
                    sortUp = !sortUp;
                }
                else
                {
                    config.sortUp = !config.sortUp;
                }
                
                Array.Sort(array);
            }
        }

        EditorGUILayout.EndHorizontal();
        return array;
    }

    protected void DrawHorizontal(float width, Action action)
    {
        EditorGUILayout.BeginHorizontal(GUILayout.Width(width));
        action.Invoke();
        EditorGUILayout.EndHorizontal();
    }

    protected void DrawVertical(float width, Action action)
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(width));
        action.Invoke();
        EditorGUILayout.EndVertical();
    }

    protected void DrawPreferredLabel(string label)
    {
        PreferredGUIUtil.DrawPreferredLabel(label);
    }
    protected int DrawPreferredInt(int value, int min = 0, int max = 64)
    {
        int temp = PreferredGUIUtil.DrawPreferredInt(value, 2);
        temp = Mathf.Clamp(temp, min, max);
        return temp;
    }

    protected int DrawLabelInt(string label, int value, int min = 0, int max = 64)
    {
        int labelWidth = Mathf.CeilToInt(GUI.skin.label.CalcSize(new GUIContent(label)).x);
        int intWidth = Mathf.CeilToInt(GUI.skin.label.CalcSize(new GUIContent(value.ToString())).x) + 4;
        EditorGUILayout.BeginHorizontal(GUILayout.Width(labelWidth + intWidth));
        EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
        int temp = EditorGUILayout.IntField(value, GUILayout.Width(intWidth));
        EditorGUILayout.EndHorizontal();
        temp = Mathf.Clamp(temp, min, max);
        return temp;
    }
    
}