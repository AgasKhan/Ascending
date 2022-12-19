using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public abstract class AbilitiesParent : ButtonColorParent, IBeginDragHandler, IDragHandler, IEndDragHandler
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

    

    protected override void Config()
    {
        base.Config();
        MyAwakes += MyAwake;
    }

    void MyAwake()
    {
        points = CSVReader.LoadFromPictionary<int>("PlayerPoints");

        myCanvasGroup = GetComponent<CanvasGroup>();

        var button = GetComponent<Button>();

        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(Listener);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].superior = allCosts[i].ToString();
        }

        originalParent = transform.parent;
    }

    public T VinculatedAbilities<T>() where T : Abilities.Ability, new()
    {
        if (Abilities.Abilitieslist.ContainsKey(typeof(T)))
        {
            myAbility = Abilities.Abilitieslist[typeof(T)];

            currentLevel = myAbility.level;

            if (currentLevel > 0)
                UnlockAbility(currentLevel);

            if (currentLevel >= buttons.Length - 1)
            {
                DetailsWindow.instance.DeactiveLevelButton();
            }
            ChangeColor(currentLevel, buttons.Length);
           
            DebugPrint.Log("lo encontro");
        }
            
        else
        {
            myAbility = new T();

            myAbility.level = (currentLevel);

            DebugPrint.Log("lo creo " + typeof(T).FullName);
        }

        return ((T)myAbility);
    }

    public virtual void Listener()
    {
        DetailsWindow.ModifyTexts(information);

        DetailsWindow.GenerateButtons(buttons);

        DetailsWindow.SetLevelUpButton(Upgrade, buttons.Length > currentLevel);
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
        ChangeColor(currentLevel, buttons.Length);
        myAbility.level = currentLevel;
    }

    public void UnlockAbility(int level)
    {
        currentLevel = level;
        myAbility.level = currentLevel;
        myCanvasGroup.interactable = true;
        myCanvasGroup.blocksRaycasts = true;
    }

    public virtual void UnlockNextButton()
    {
        var aux = originalParent.GetSiblingIndex() + 1 ;
        var nextButton = originalParent.parent.GetChild(aux).GetComponentInChildren<AbilitiesParent>();
        nextButton.UnlockAbility(0);
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
        if (currentLevel < 1)
            return;

        parentAfterDrag = transform.parent;
        transform.SetParent(transform.parent.parent.parent.parent);
        transform.SetAsLastSibling();
        myCanvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentLevel < 1)
            return;

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentLevel < 1)
            return;

        transform.SetParent(parentAfterDrag);
        myCanvasGroup.blocksRaycasts = true;
    }

    public virtual void ActiveAbility()
    {
        myAbility.active = true;
    }

    public virtual void DeactiveAbility()
    {
        myAbility.active = false;
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