using System;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class IntSwitch : MonoBehaviour
{
    [SerializeField] SDFBtn leftBtn;
    [SerializeField] SDFBtn rightBtn;
    public TextMeshProUGUI title;

    int crt; int max = 9; int min;

    public Action<int> switchCallBack;

    private void Awake()
    {
        ClickListener.Add(leftBtn.transform).onClick = OnClickLeft;
        ClickListener.Add(rightBtn.transform).onClick = OnClickRight;
    }

    public void Set(int crt, int max, int min = 0)
    {
        this.crt = crt;
        this.max = max;
        this.min = min;
        this.crt = Mathf.Max(this.crt, this.min);
        this.crt = Mathf.Min(this.crt, this.max);
        if (title != null)
        {
            title.text = crt.ToString();
        }
        switchCallBack?.Invoke(crt);
    }

    private void OnClickLeft()
    {
        crt--;
        if (crt < min)
        {
            crt = max;
        }
        if (title != null)
        {
            title.text = crt.ToString();
        }
        switchCallBack?.Invoke(crt);
    }

    private void OnClickRight()
    {
        crt++;
        if (crt > max)
        {
            crt = min;
        }
        if (title != null)
        {
            title.text = crt.ToString();
        }
        switchCallBack?.Invoke(crt);
    }
}
