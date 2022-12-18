using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public abstract class AbilitiesParent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    

    static int points;

    [HideInInspector]
    public Transform parentAfterDrag;
    [HideInInspector]
    public CanvasGroup myCanvasGroup;

    public Abilities.Ability myAbility;

    public int[] allCosts;
    public DoubleString information;
    public DoubleString[] buttons;

    public Transform originalParent;
    public int currentLevel = 0;

    #region Colores
    public enum Level
    {
        Default = 0,
        Blue = 1,
        Green = 2,
        Yellow = 3,
        Violet = 4
    }
    
    Image myImage;

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
                auxColor = Color.blue;
                break;
            case "Green":
                auxColor = Color.green;
                break;
            case "Yellow":
                auxColor = Color.yellow;
                break;
            case "Violet":
                auxColor = Color.magenta;
                break;
        }

        Utilitys.LerpInTime(myImage.fillAmount, 0, 0.3f, Mathf.Lerp, (save) => { myImage.fillAmount = save; });

        TimersManager.Create(0.3f,
            ()=> 
            {
                myImage.fillOrigin = 1;
                myImage.color = auxColor;
                Utilitys.LerpInTime(myImage.fillAmount, 1, 0.3f, Mathf.Lerp, (save) => { myImage.fillAmount = save; });
            });

        //Utilitys.LerpInTime(myImage.color, auxColor, 0.3f, Color.Lerp, (save) => { myImage.color = save; });
    }

    public void ChangeColor()
    {
        //currentLevel

        //Enum.GetNames(Level)[currentLevel];

        var arr = Enum.GetValues(typeof(Level));

        int aux = 4 - ((buttons.Length) - currentLevel);

        ChangeColor((Level)arr.GetValue(aux));
        
    }


    #endregion

    private void Awake()
    {
        points = CSVReader.LoadFromPictionary<int>("PlayerPoints");

        myCanvasGroup = GetComponent<CanvasGroup>();
        myImage = GetComponent<Image>();

        var button = GetComponent<Button>();

        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(Listener);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].superior = allCosts[i].ToString();
        }

        originalParent = transform.parent;
    }

    public void VinculatedAbilities<T>() where T : Abilities.Ability, new()
    {
        if (Abilities.Abilitieslist.ContainsKey(typeof(T)))
        {
            myAbility = Abilities.Abilitieslist[typeof(T)];
            DebugPrint.Log("lo encontro");
        }
            
        else
        {
            myAbility = new T();

            myAbility.level = (currentLevel);

            DebugPrint.Log("lo creo " + typeof(T).FullName);
        }
    }

    public virtual void Listener()
    {
        DetailsWindow.ModifyTexts(information);

        DetailsWindow.GenerateButtons(buttons);

        DetailsWindow.SetLevelUpButton(Upgrade);
    }

    public virtual void Upgrade()
    {
        if(currentLevel < buttons.Length-1)
        {
            NormalUpgrade();
        }
        else
        {
            if ((originalParent.parent.childCount - 1) > originalParent.GetSiblingIndex())
                UnlockNextButton();
            NormalUpgrade();
            DetailsWindow.instance.DeactiveLevelButton();
        }
    }

    public void NormalUpgrade()
    {
        CheckPoints(allCosts[currentLevel]);
        currentLevel++;
        ChangeColor();

        myAbility.level = currentLevel;
    }

    public void UnlockAbility()
    {
        currentLevel = 0;
        myAbility.level = currentLevel;
        myCanvasGroup.interactable = true;
        myCanvasGroup.blocksRaycasts = true;
    }

    public virtual void UnlockNextButton()
    {
        var aux = originalParent.GetSiblingIndex() + 1 ;
        var nextButton = originalParent.parent.GetChild(aux).GetComponentInChildren<AbilitiesParent>();
        nextButton.UnlockAbility();
    }

    public void CheckPoints(int cost)
    {
        if (points >= cost)
        {
            points -= cost;
            CSVReader.SaveInPictionary<int>("PlayerPoints", points);

            DetailsWindow.instance.RefreshPoints();
        }
        else
            print("No tienes puntos suficientes");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.parent.parent.parent.parent);
        transform.SetAsLastSibling();
        myCanvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        myCanvasGroup.blocksRaycasts = true;
    }

    public virtual void ActiveAbility()
    {
        myAbility.active = true;
    }


}

[System.Serializable]
public struct DoubleString
{
    [TextArea(1, 6)]
    public string superior;

    [TextArea(3, 6)]
    public string inferior;

    public DoubleString(string superior, string inferior)
    {
        this.superior = superior;
        this.inferior = inferior;
    }
}