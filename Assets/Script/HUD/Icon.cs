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

    public void LerpFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
    {
        Utilitys.LerpInTime(back.color, targetColor, duration, Color.Lerp,
            (saveColor) =>
            {
                back.color = saveColor;
            });

        Utilitys.LerpInTime(front.color, targetColor, duration, Color.Lerp,
            (saveColor) =>
            {
                front.color = saveColor;
            });
    }

    public void LerpFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
    {
        Utilitys.LerpInTime(back.color, new Color(back.color.r, back.color.g, back.color.b, alpha), duration, Color.Lerp, 
            (saveColor)=> 
            { 
                back.color = saveColor;
            });

        Utilitys.LerpInTime(front.color, new Color(front.color.r, front.color.g, front.color.b, alpha), duration, Color.Lerp, 
            (saveColor) =>
            {
                front.color = saveColor;
            });
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
        if (textManager.Load() && textManager.timeInScreen.Chck)
            textManager.Next();

        textManager.Fade();
    }
}
