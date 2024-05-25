using TMPro;
using UnityEngine;

public class LanguageText : TextMeshProUGUI
{
    [ReadOnlyProperty]
    public int languageID = 10021;

    protected override void Awake()
    {
        base.Awake();
        if (Application.isPlaying)
        {
            RefreshText();
        }
    }

    public void RefreshText()
    {
        text = LanguageModel.Get(languageID);
    }
}