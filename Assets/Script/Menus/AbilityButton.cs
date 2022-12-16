using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityButton : SkillTreeManager, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool lockedAbility = false;
    public string lockedMessage;

    [HideInInspector] 
    public Transform parentAfterDrag;
    [HideInInspector]
    public CanvasGroup myCanvasGroup;
    [HideInInspector] 
    public Level myLevel;

    Image myImage;
    Button myButton;

    public enum Level
    {
        Default = 0,
        Blue = 1,
        Green = 2,
        Yellow = 3,
        Violet = 4
    }
    int currentColor = 0;
    Level[] myColors = { Level.Default, Level.Blue, Level.Green, Level.Yellow, Level.Violet };
    
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

    private void Awake()
    {
        myCanvasGroup = GetComponent<CanvasGroup>();
        myImage = GetComponent<Image>();
        myButton = GetComponent<Button>();

        if (myLevel != default)
        {
            ChangeColor(myLevel);
        }

        myButton.onClick.RemoveAllListeners();
        if (lockedAbility)
        {
            myButton.onClick.AddListener(() =>
            {
                OpenLockedDetails(transform.name, lockedMessage);
            });
            myCanvasGroup.blocksRaycasts = false;
        }
        else
        {
            myButton.onClick.AddListener(() =>
            {
                OpenDetailWindow(transform.name);
            });
        }


    }

    public void ChangeColor(Level l)
    {
        
        string aux = l.ToString();
        switch(aux)
        {
            case "Default":
                myImage.color = Color.white;
                break;
            case "Blue":
                myImage.color = Color.blue;
                break;
            case "Green":
                myImage.color = Color.green;
                break;
            case "Yellow":
                myImage.color = Color.yellow;
                break;
            case "Violet":
                myImage.color = Color.magenta;
                break;
        }
        
    }
    public void ChangeColor()
    {
        currentColor++;
        ChangeColor(myColors[currentColor]);
    }


    public void RefreshAbilitiesList()
    {
        CSVReader.SaveInPictionary<bool>(transform.name + "IsActive", true);
    }

}

