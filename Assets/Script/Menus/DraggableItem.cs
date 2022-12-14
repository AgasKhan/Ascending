using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentAfterDrag;
    public CanvasGroup myCanvasGroup;
    public Level myLevel;

    Image myImage;

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

        if (myLevel != default)
        {
            ChangeColor(myLevel);
        }
    }

    public void ChangeColor(Level l)
    {
        
        string aux = l.ToString();
        switch(aux)
        {
            case "Default":
                myImage.color = Color.red;
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
        
    }

}
