using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class ButtonColorParent : MyScripts
{
    Image myImage;

    // Start is called before the first frame update

    #region Colores
    public enum Level
    {
        Default = 0,
        Blue = 1,
        Green = 2,
        Yellow = 3,
        Violet = 4
    }

    public void ChangeColor(Level l)
    {
        myImage.type = Image.Type.Filled;
        myImage.fillMethod = Image.FillMethod.Horizontal;
        myImage.fillOrigin = 0;

        string aux = l.ToString();
        Color auxColor = Color.white;

        switch (aux)
        {
            case "Blue":
                auxColor = Color.blue;  //new Color (22,55,82);
                break;
            case "Green":
                auxColor = Color.green; //new Color(22,82,44);
                break;
            case "Yellow":
                auxColor = Color.yellow; //new Color(82,62,22);
                break;
            case "Violet":
                auxColor = Color.magenta; //new Color(62,22,82);
                break;
        }

        Utilitys.LerpInTime(myImage.fillAmount, 0, 0.3f, Mathf.Lerp, (save) => { myImage.fillAmount = save; });

        TimersManager.Create(0.3f,
            () =>
            {
                myImage.fillOrigin = 1;
                myImage.color = auxColor;
                Utilitys.LerpInTime(myImage.fillAmount, 1, 0.3f, Mathf.Lerp, (save) => { myImage.fillAmount = save; });
            });

        //Utilitys.LerpInTime(myImage.color, auxColor, 0.3f, Color.Lerp, (save) => { myImage.color = save; });
    }

    public void ChangeColor(int l, int elementsCount)
    {
        //currentLevel

        //Enum.GetNames(Level)[currentLevel];

        if (l > 0)
        {
            var arr = Enum.GetValues(typeof(Level));

            int aux = 4 - ((elementsCount) - l);

            ChangeColor((Level)arr.GetValue(aux));
        }
    }
    #endregion

    protected override void Config()
    {
        MyAwakes += MyAwake;
    }

    void MyAwake()
    {

        myImage = GetComponent<Image>();
    }

}
