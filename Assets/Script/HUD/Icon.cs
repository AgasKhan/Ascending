using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Internal;

public class Icon : MonoBehaviour
{

    public Image back;
    public Image front;
    public TextManager textManager;

    public void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
    {
        back.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
        front.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
    }

    public void CrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
    {
        back.CrossFadeAlpha(alpha, duration, ignoreTimeScale);
        front.CrossFadeAlpha(alpha, duration, ignoreTimeScale);
    }

    public void ChangeFront(Sprite s)
    {
        front.sprite = s;
    }

    public void ChangeBack(Sprite s)
    {
        back.sprite = s;
    }

    void Awake()
    {
        textManager.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (textManager.Load() && textManager.timeInScreen.Chck())
            textManager.Next();

        textManager.Fade();
    }
}
