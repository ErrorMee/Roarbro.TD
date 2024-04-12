using EasingCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Breathing : MonoBehaviour
{
    private Graphic graphic;
    [Range(0.1f, 1)]
    public float sycle = 0.4f;

    float alpha = 1;

    bool fadeOut = true;

    public Ease ease = Ease.Linear;

    void Start()
    {
        graphic = GetComponent<Graphic>();
        alpha = graphic.color.a * sycle;
        fadeOut = true;
    }

    void LateUpdate()
    {
        if (fadeOut)
        {
            alpha -= Time.deltaTime;
            if (alpha < 0)
            {
                fadeOut = false;
            }
        }
        else
        {
            alpha += Time.deltaTime;
            if (alpha > sycle)
            {
                fadeOut = true;
            }
        }
        Color color = graphic.color;
        color.a = Easing.Get(ease).Invoke(Mathf.Clamp01(alpha / sycle));
        graphic.color = color;
    }
}
