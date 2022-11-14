using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpritesManager : MonoBehaviour
{
    static public SpritesManager instance;

    public PowerSelecctor powerSelector;

    public GameObject powerList;

    public GameObject debuffList;

    public GameObject buffList;

    public List<Image> power;

    public List<Image> debuff;

    public List<Image> buff;

    int iDebuff;

    int iBuff;

    //para un solo poder
    static public void RefreshPower()
    {
        for (int i = 0; i < GameManager.player.power.Count; i++)
        {
            instance.power[i * 2 + 1].sprite = GameManager.player.power[i].ui.GeneralIcon;
        }

        instance.powerList.SetActive(true);
        instance.powerSelector.gameObject.SetActive(true);
    }

    static public void ChangePowerSelector()
    {
        AnimPowerSelector("New");
        instance.powerSelector.enabled = true;
        instance.powerSelector.PowerSlctrPos(instance.power[GameManager.player.actualPower * 2 + 1].transform.position);
        
        
    }

    static public void AnimPowerSelector(string anim)
    {
        instance.powerSelector.Animation(anim);
    }

    static public void AddDebuff(Sprite sprite)
    {
        foreach (var item in instance.debuff)
        {
            if (item.sprite == sprite)
            {
                return;
            }
        }

        instance.iDebuff++;
        instance.debuff[instance.iDebuff].sprite = sprite;

        instance.debuff[instance.iDebuff].CrossFadeAlpha(1,0.3f,false);
    }

    static public void RemoveDebuff(Sprite sprite)
    {
        instance.iDebuff--;

        foreach (var item in instance.debuff)
        {
            if(item.sprite==sprite)
            {
                item.CrossFadeAlpha(0, 0, false);
                item.transform.GetChild(0).SetAsLastSibling();
                return;
            }
        }
    }

    static public int AddBuff()
    {
        foreach (var item in instance.buff)
        {
            if (item.sprite == GameManager.player.power[GameManager.player.actualPower].ui.ActiveIcon)
            {
                return-1;
            }
        }

        instance.buff[instance.iBuff].sprite = GameManager.player.power[GameManager.player.actualPower].ui.ActiveIcon;
        instance.buff[instance.iBuff].CrossFadeColor(Color.white, 0.3f, true, false);
        instance.buff[instance.iBuff].CrossFadeAlpha(1, 0.3f, false);
        instance.iBuff++;
        return instance.iBuff-1;
    }

    static public void RemoveAllBuffs()
    {
        instance.iBuff=0;
        foreach (var item in instance.buff)
        {
            item.sprite = null; 
            item.CrossFadeColor(Color.black, 0.3f, true, false);
            item.CrossFadeAlpha(0,0.3f,false);
            
        }
    }

    private void Awake()
    {
        instance = this;

        power.AddRange(powerList.GetComponentsInChildren<Image>());

        debuff.AddRange(debuffList.GetComponentsInChildren<Image>());

        buff.AddRange(buffList.GetComponentsInChildren<Image>());

        iDebuff = 0;
        iBuff = 0;

        for (int i = 1; i < 1; i++)
        {
            powerList.transform.GetChild(i).gameObject.SetActive(true);
        }

        //ChangePowerSelector();

        //powerSelector.PowerSlctrPos(Vector3.right* 1160 + Vector3.up * 125);
    }
}