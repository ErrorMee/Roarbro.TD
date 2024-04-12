
using UnityEngine;
using UnityEditor;

public class WidgetEditor
{
    public const string rootMenu = "GameObject/Prefab/♦ Widget/";

    [MenuItem(rootMenu + "Img", false, 1)]
    static void Img(MenuCommand menuCmd)
    {
        AddWidget(menuCmd, "Img");
    }

    [MenuItem(rootMenu + "Btn", false, 2)]
    static void Btn(MenuCommand menuCmd)
    {
        AddWidget(menuCmd, "Btn");
    }

    [MenuItem(rootMenu + "Txt", false, 3)]
    static void Txt(MenuCommand menuCmd)
    {
        AddWidget(menuCmd, "Txt");
    }

    [MenuItem(rootMenu + "Lable", false, 4)]
    static void Lable(MenuCommand menuCmd)
    {
        AddWidget(menuCmd, "Lable");
    }

    [MenuItem(rootMenu + "Language", false, 5)]
    static void Language(MenuCommand menuCmd)
    {
        AddWidget(menuCmd, "Language");
    }

    [MenuItem(rootMenu + "TxtBtn", false, 6)]
    static void TxtBtn(MenuCommand menuCmd)
    {
        AddWidget(menuCmd, "TxtBtn");
    }

    [MenuItem(rootMenu + "Frame", false, 7)]
    static void Frame(MenuCommand menuCmd)
    {
        AddWidget(menuCmd, "Frame");
    }

    [MenuItem(rootMenu + "Attr", false, 8)]
    static void Attr(MenuCommand menuCmd)
    {
        AddWidget(menuCmd, "Attr");
    }

    [MenuItem(rootMenu + "Progress", false, 9)]
    static void Progress(MenuCommand menuCmd)
    {
        AddWidget(menuCmd, "Progress");
    }

    [MenuItem(rootMenu + "ScrollVertical", false, 11)]
    static void ScrollVertical(MenuCommand menuCmd)
    {
        AddWidget(menuCmd, "ScrollVertical");
    }

    [MenuItem(rootMenu + "ScrollHorizontal", false, 12)]
    static void ScrollHorizontal(MenuCommand menuCmd)
    {
        AddWidget(menuCmd, "ScrollHorizontal");
    }

    [MenuItem(rootMenu + "ScrollFocus", false, 13)]
    static void ScrollFocus(MenuCommand menuCmd)
    {
        AddWidget(menuCmd, "ScrollFocus");
    }

    [MenuItem(rootMenu + "ListView", false, 14)]
    static void ListView(MenuCommand menuCmd)
    {
        AddWidget(menuCmd, "ListView");
    }

    [MenuItem(rootMenu + "IntSwitch", false, 16)]
    static void Pages(MenuCommand menuCmd)
    {
        AddWidget(menuCmd, "IntSwitch");
    }

    static void AddWidget(MenuCommand menuCmd, string name)
    {
        GameObject select = menuCmd.context as GameObject;

        if (!select.TryGetComponent<RectTransform>(out var rectTransform))
        {
            return;
        }

        string assetPath = string.Format("Assets/Art/Window/Widget/{0}.prefab", name);
        string resourcePath = string.Format("Assets/Resources/{0}.prefab", name);

        AssetDatabase.MoveAsset(assetPath, resourcePath);

        GameObject prefab = (GameObject)Resources.Load(name);

        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, rectTransform);

        Selection.activeObject = instance;

        AssetDatabase.MoveAsset(resourcePath, assetPath);
    }
}