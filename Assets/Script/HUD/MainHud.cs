using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainHud : MonoBehaviour
{
    public struct OriginalGraphics
    {
        public Color originalColor;

        public Graphic graphic;

        public OriginalGraphics(Graphic graphic)
        {
            originalColor = graphic.color;
            this.graphic = graphic;
        }
    }



    static MainHud instance;

    public PowerSelecctor powerSelector;

    public GameObject powerList;

    public GameObject debuffList;

    public GameObject buffList;

    public Image puntero;

    public Image reticulaCharge;

    public Image reticula;

    public CanvasGroup reticulaAlpha;

    public Icon dagger;

    public List<Icon> power;

    public List<Icon> debuff;

    public List<Icon> buff;

    public Animator animatorReticula;

    public CanvasScaler canvas;

    public Color cancelColor;

    Color originalColor;

    Routine punteroRoutine;

    int iDebuff;

    int iBuff;

    OriginalGraphics[] graphics;

    Timer tim;

    string preciusNamePlay;

    static public void ReticulaCrossColor(Color color)
    {
        instance.reticula.color = color;
    }

    static public Color ReticulaCrossColor()
    {
        return instance.reticula.color;
    }

    static public void ReticulaAlpha(float n)
    {
        instance.reticulaAlpha.alpha = n;
    }

    static public void ReticulaPlay(string name)
    {
        if(instance.preciusNamePlay !=name)
        {
            instance.animatorReticula.Play(name);
            instance.preciusNamePlay = name;
        }   
    }

    static public void ReticulaFill(float n)
    {
        instance.reticulaCharge.fillAmount = n;
    }

    static public Image Puntero()
    {
        return instance.puntero;
    }

    static public Vector2 Center()
    {
        return (instance.canvas.referenceResolution * (1/2f));
    }

    static public void PunteroPos()
    {
        instance.puntero.rectTransform.position = Center();
        instance.puntero.color = instance.originalColor;
    }

    static public void PunteroPos(Vector3 vec)
    {
        instance.puntero.rectTransform.position = vec;
        instance.puntero.color = instance.cancelColor;

        if (instance.punteroRoutine == null || instance.punteroRoutine.Chck)
            instance.punteroRoutine = TimersManager.Create(0.1f, PunteroPos);
        else
            instance.punteroRoutine.Reset();


    }

    static public void RefreshPowersUI()
    {
        RefreshPower();
        ChangePowerSelector();
    }

    static public void RestoreOriginalColorWithFade(float time, string nameToNotIgnore)
    {
        foreach (var item in instance.graphics)
        {
            if (item.graphic.transform.parent.name != nameToNotIgnore)
            {
                Utilitys.LerpInTime(item.graphic.color, item.originalColor, time, Color.Lerp, (colorSave) => { item.graphic.color = colorSave; });
            }
        }
    }

    static public void ChangeAlphaWithFade(float alpha, float time, string nameToNotIgnore)
    {
        foreach (var item in instance.graphics)
        {
            if (item.graphic.transform.parent.name != nameToNotIgnore)
            {
                var color = item.graphic.color;

                var destinyColor = new Color(color.r, color.g, color.b, alpha);

                Utilitys.LerpInTime(color, destinyColor, time, Color.Lerp, (colorSave) => { item.graphic.color = colorSave;});
            }
        }
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
            if (item.front.sprite == GameManager.player.power[GameManager.player.actualPower].ui.ActiveIcon)
            {
                return -1;
            }
        }

        instance.buff[instance.iBuff].textManager.ShowText(false, GameManager.player.power[GameManager.player.actualPower].ui.activeText);
        instance.buff[instance.iBuff].ChangeFront(GameManager.player.power[GameManager.player.actualPower].ui.ActiveIcon);

        instance.buff[instance.iBuff].LerpFadeColor(Color.white, 0.3f, true, false);
        //instance.buff[instance.iBuff].LerpFadeAlpha(1, 0.3f, false);

        instance.iBuff++;
        if (instance.buff.Count == instance.iBuff)
            instance.iBuff = 0;

        return instance.iBuff - 1;
    }

    static public void RemoveAllBuffs()
    {
        instance.iBuff = 0;
        foreach (var item in instance.buff)
        {
            item.front.color = new Color(0,0,0,0);
            item.ChangeFront(null);
            Utilitys.LerpInTime(item.back.color, item.front.color, 0.3f, Color.Lerp, (save)=> { item.back.color = save; });
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

        instance.debuff[instance.iDebuff].LerpFadeAlpha(1, 0.3f, false);
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

        var auxGraphics = GetComponentsInChildren<Graphic>();

        graphics = new OriginalGraphics[auxGraphics.Length];

        canvas = GetComponent<CanvasScaler>();

        power.AddRange(powerList.GetComponentsInChildren<Icon>());

        debuff.AddRange(debuffList.GetComponentsInChildren<Icon>());

        buff.AddRange(buffList.GetComponentsInChildren<Icon>());

        iDebuff = 0;
        iBuff = 0;

        for (int i = 1; i < 1; i++)
        {
            powerList.transform.GetChild(i).gameObject.SetActive(true);
        }

        for (int i = 0; i < auxGraphics.Length; i++)
        {
            graphics[i] = new OriginalGraphics(auxGraphics[i]);
        }

        tim = TimersManager.Create(1);

        ChangeAlphaWithFade(0, 0, "");

        StartCoroutine(posStart());

    }


    private void Start()
    {
        originalColor = puntero.color;
        DaggerText(0, 0);
        dagger.textManager.timeInScreen.Stop();
        ReticulaFill(0);
        ReticulaAlpha(CSVReader.LoadFromPictionary<float>("AlphaReticula", 0.75f));
    }

    IEnumerator posStart()
    {
        while (!tim.Chck)
        {
            yield return null;
        }

        RestoreOriginalColorWithFade(2, "Effect");

    }

    IEnumerator RetardedTim()
    {
        yield return null;
        instance.powerSelector.PowerSlctrPos(instance.power[GameManager.player.actualPower].transform.position);
    }


    #endregion
}
