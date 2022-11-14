using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainHud : MonoBehaviour
{
    Graphic[] graphics;

    Timer tim;

    static public void RefreshUI()
    {
        SpritesManager.RefreshPower();
        SpritesManager.ChangePowerSelector();

        TextCanvas.SrchMessages("ActualPower").ShowText(true,GameManager.player.power[GameManager.player.actualPower].ui.generalText);
    }

    static public void AddBuff()
    {
        SpritesManager.AnimPowerSelector("Active");
        int aux = SpritesManager.AddBuff();

        if(aux>=0)
            TextCanvas.SrchMessages("buff"+(aux+1).ToString()).ShowText(false, GameManager.player.power[GameManager.player.actualPower].ui.activeText);
    }

    static public void RemoveAllBuffs()
    {
        SpritesManager.RemoveAllBuffs();
    }


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
            if(item.transform.parent.name != "Effect")
                item.CrossFadeAlpha(2, 1, false);
        }

    }
}
