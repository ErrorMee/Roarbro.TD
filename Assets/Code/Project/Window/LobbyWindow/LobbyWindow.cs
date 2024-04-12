using DG.Tweening;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class LobbyWindow : WindowBase
{
    [SerializeField] SDFImg icon;
    [SerializeField] SDFBtn startBtn;
    [SerializeField] SDFBtn optionBtn;
    [SerializeField] SDFBtn quitBtn;

    override protected void Awake()
    {
        base.Awake();

        ArchiveModel.Instance.Init();

        ClickListener.Add(startBtn.transform).onClick = OnClickStart;
        ClickListener.Add(optionBtn.transform).onClick = OnClickOption;
        ClickListener.Add(quitBtn.transform).onClick = OnClickQuit;
        ClickListener.Add(icon.transform).onClick = OnClickIcon;
    }

    public override void OnOpen(object obj)
    {
        base.OnOpen(obj);

        int iconIndex = ArchiveModel.GetInt(ArchiveEnum.IconIndex, 0, false);
        icon.ID = iconIndex;

    }

    void OnClickStart()
    {
        WindowModel.Open(WindowEnum.Archive);
    }

    void OnClickIcon()
    {
        int iconIndex = ArchiveModel.GetInt(ArchiveEnum.IconIndex, 0, false);
        iconIndex++;
        if (iconIndex > 10)
        {
            iconIndex = 0;
        }
        icon.ID = iconIndex;
        ArchiveModel.SetInt(ArchiveEnum.IconIndex, iconIndex, false);
    }

    void OnClickQuit()
    {
        WindowModel.Dialog(LanguageModel.Get(10022), LanguageModel.Get(10027), OnConfirm, OnCancel);
    }

    private void OnConfirm()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    private void OnCancel() { }

    void OnClickOption()
    {
        WindowModel.Open(WindowEnum.Option);
    }
}