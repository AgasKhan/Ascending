using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainHud : MonoBehaviour
{
    Graphic[] graphics;

    Timer tim;

    void Start()
    {
        graphics = GetComponentsInChildren<Graphic>();

        tim = Timers.Create(1);

        foreach (var item in graphics)
        {
            item.CrossFadeAlpha(0, 0, false);
        }

        StartCoroutine(posStart());
        
    }

    IEnumerator posStart()
    {
        while (!tim.Chck())
        {
            yield return null;
        }

        foreach (var item in graphics)
        {
            item.CrossFadeAlpha(2, 1, false);
        }

    }
}
