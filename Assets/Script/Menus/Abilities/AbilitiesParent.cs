using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class AbilitiesParent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector]
    public Transform parentAfterDrag;
    [HideInInspector]
    public CanvasGroup myCanvasGroup;

    protected Abilities.Ability myAbility;

    [System.Serializable]
    public struct DoubleString
    {
        public string superior;

        [TextArea(3, 6)]
        public string inferior;
    }

    public DoubleString information;
    public DoubleString[] buttons;

    private void Awake()
    {
        myCanvasGroup = GetComponent<CanvasGroup>();


        var button = GetComponent<Button>();

        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(Listener);
    }

    protected void Start()
    {
        myAbility = new Abilities.Ability(this.GetType(), 0, false);
    }

    public virtual void Listener()
    {
        DetailsWindow.instance.ModifyTexts(information);

        DetailsWindow.instance.GenerateButtons(buttons);

        DetailsWindow.instance.SetLevelUpButton();
    }

    /*
    public void CheckPoints()
    {
        var aux = CSVReader.LoadFromPictionary<int>("PlayerPoints");
        if (aux >= cost)
        {
            aux -= cost;
            CSVReader.SaveInPictionary<int>("PlayerPoints", aux);

            if (quantity == 0)
                ImproveSkills(myAbility.ToString(), activate);
            else
                ImproveSkills(myAbility.ToString(), quantity);

            UnlockNextButton();
            RefreshPoints();
        }
        else
            print("No tienes puntos suficientes");

    }*/


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


}


[System.Serializable]
public static class Abilities
{
    [System.Serializable]
    public class Ability
    {
        public float level;
        public bool active;

        public Ability(System.Type tipo, float level, bool active)
        {
            this.level = level;
            this.active = active;

            list.Add(tipo, this);
        }
    }

    public static Pictionarys<System.Type, Ability> list;
}

