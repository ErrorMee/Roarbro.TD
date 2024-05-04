using System;
using System.Collections.Generic;
/// <summary>
/// Model.Static is used to simplify the use of model
/// </summary>
public partial class WindowModel
{
    public static WindowLayer GetLayer(WindowLayerEnum windowLayer)
    {
        return Instance.GetWindowLayer(windowLayer);
    }

    public static WindowBase Get(int id)
    {
        return Instance.GetWindow(id);
    }

    public static bool Has(WindowEnum window)
    {
        return Instance.HasWindow((int)window);
    }

    public static void Open(int id, object obj = null)
    {
        Instance.OpenWindow(id, obj);
    }

    public static void Open(WindowEnum window, object obj = null)
    {
        Open((int)window, obj);
    }

    public static void Close(int id)
    {
        if (Instance != null)
        {
            Instance.CloseWindow(id);
        }
    }

    public static void Close(WindowEnum window)
    {
        Close((int)window);
    }

    public static void Msg(string msg)
    {
        Open(WindowEnum.Msg, msg);
    }

    public static void Wait(bool open)
    {
        if (open)
        {
            Open(WindowEnum.Wait);
        }
        else
        {
            Close(WindowEnum.Wait);
        }
    }

    /// <summary>
    /// title: LanguageModel.Instance.Get(20010)
    /// </summary>
    /// <param name="title"> LanguageModel.Instance.Get(20010)</param>
    public static void Dialog(string title, string content, Action confirm,
        Action cancel = null)
    {
        DialogInfo dialogInfo = SharedPool<DialogInfo>.Get();

        dialogInfo.content = content;
        dialogInfo.confirm = confirm;
        dialogInfo.cancel = cancel;
        dialogInfo.title = title;

        Open(WindowEnum.Dialog, dialogInfo);
    }

    public static void CloseAll()
    {
        foreach (WindowLayerEnum value in Enum.GetValues(typeof(WindowLayerEnum)))
        {
            WindowLayer layer = Instance.GetWindowLayer(value);

            LinkedList<WindowBase> windows = layer.GetWindows();

            while (windows.Count > 0 && windows.Last != null)
            {
                Close(windows.Last.Value.config.id);
            }

            windows.Clear();
        }
    }
}