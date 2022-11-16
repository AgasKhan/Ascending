using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainHud : MonoBehaviour
{
    static MainHud instance;

    public PowerSelecctor powerSelector;

    public GameObject powerList;

    public GameObject debuffList;

    public GameObject buffList;

    public Icon dagger;

    public List<Icon> power;

    public List<Icon> debuff;

    public List<Icon> buff;

    int iDebuff;

    int iBuff;

    Graphic[] graphics;

    Timer tim;


    static public void RefreshUI()
    {
        RefreshPower();
        ChangePowerSelector();
    }

    #region Daggers

    static public void DaggerPower(float fill)
    {
        instance.dagger.front.fillAmount = fill;
    }

    static public void DaggerText(int actual, int total)
    {
        string text = actual + "/" + total;

        if (actual <= 0)
            text = text.RichText("color", "red");

        instance.dagger.textManager.ShowText(false, text);
    }

    #endregion

    #region powers

    static public void AnimPowerSelector(string anim)
    {
        instance.powerSelector.Animation(anim);
    }

    static public void RefreshPower()
    {
        instance.powerList.SetActive(true);
        instance.powerSelector.gameObject.SetActive(true);

        for (int i = 0; i < GameManager.player.power.Count; i++)
        {
            instance.power[i].ChangeFront(GameManager.player.power[i].ui.GeneralIcon);
            instance.power[i].textManager.ShowText(true, GameManager.player.power[i].ui.generalText);
        }
    }

    static public void ChangePowerSelector()
    {
        AnimPowerSelector("New");
        instance.powerSelector.enabled = true;

        instance.StartCoroutine(instance.RetardedTim());

    }

    #endregion

    #region buffs attacks
    static public void AddBuff()
    {
        AnimPowerSelector("Active");
        AddBuffSIcon();
    }

    static public int AddBuffSIcon()
    {
        foreach (var item in instance.buff)
        {
            if (item.front == GameManager.player.power[GameManager.player.actualPower].ui.ActiveIcon)
            {
                return -1;
            }
        }

        instance.buff[instance.iBuff].textManager.ShowText(false, GameManager.player.power[GameManager.player.actualPower].ui.activeText);
        instance.buff[instance.iBuff].ChangeFront(GameManager.player.power[GameManager.player.actualPower].ui.ActiveIcon);

        instance.buff[instance.iBuff].CrossFadeColor(Color.white, 0.3f, true, false);
        instance.buff[instance.iBuff].CrossFadeAlpha(1, 0.3f, false);
        instance.iBuff++;
        return instance.iBuff - 1;
    }

    static public void RemoveAllBuffs()
    {
        instance.iBuff = 0;
        foreach (var item in instance.buff)
        {
            item.ChangeFront(null);
            item.front.CrossFadeAlpha(0, 0, false);
            item.back.CrossFadeColor(Color.black, 0.3f, true, false);
            item.back.CrossFadeAlpha(0, 0.3f, false);
        }
    }

    #endregion

    #region debuffs player
    static public void AddDebuff(Sprite sprite)
    {
        foreach (var item in instance.debuff)
        {
            if (item.front == sprite)
            {
                return;
            }
        }

        instance.iDebuff++;
        instance.debuff[instance.iDebuff].ChangeFront(sprite);

        instance.debuff[instance.iDebuff].CrossFadeAlpha(1, 0.3f, false);
    }

    static public void RemoveDebuff(Sprite sprite)
    {
        instance.iDebuff--;

        foreach (var item in instance.debuff)
        {
            if (item.front == sprite)
            {
                item.front.CrossFadeAlpha(0, 0, false);
                item.transform.GetChild(0).SetAsLastSibling();
                return;
            }
        }
    }

    #endregion

    #region unity functions

    void Awake()
    {
        instance = this;

        graphics = GetComponentsInChildren<Graphic>();

        power.AddRange(powerList.GetComponentsInChildren<Icon>());

        debuff.AddRange(debuffList.GetComponentsInChildren<Icon>());

        buff.AddRange(buffList.GetComponentsInChildren<Icon>());

        iDebuff = 0;
        iBuff = 0;

        for (int i = 1; i < 1; i++)
        {
            powerList.transform.GetChild(i).gameObject.SetActive(true);
        }

        tim = Timers.Create(1);

        foreach (var item in graphics)
        {
            item.CrossFadeAlpha(0, 0, false);
        }

        StartCoroutine(posStart());

    }


    private void Start()
    {
        DaggerText(0, 0);
        dagger.textManager.timeInScreen.Stop();
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

    IEnumerator RetardedTim()
    {
        yield return null;
        instance.powerSelector.PowerSlctrPos(instance.power[GameManager.player.actualPower].transform.position);
    }


    #endregion
}
