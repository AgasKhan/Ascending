using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class AbilitiesParent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [System.Serializable]
    public struct DoubleString
    {
        public string superior;

        [TextArea(3, 6)]
        public string inferior;
    }


    [HideInInspector]
    public Transform parentAfterDrag;
    [HideInInspector]
    public CanvasGroup myCanvasGroup;

    public Abilities.Ability myAbility;

    public int[] allCosts;
    public DoubleString information;
    public DoubleString[] buttons;

    public Transform originalParent;

    #region Colores
    public enum Level
    {
        Default = 0,
        Blue = 1,
        Green = 2,
        Yellow = 3,
        Violet = 4
    }
    public int currentLevel = 0;
    Level[] myColors = { Level.Default, Level.Blue, Level.Green, Level.Yellow, Level.Violet };
    Image myImage;

    public void ChangeColor(Level l)
    {

        string aux = l.ToString();
        switch (aux)
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
        ChangeColor(myColors[currentLevel]);
    }


    #endregion

    private void Awake()
    {
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

        if (Abilities.Abilitieslist.ContainsKey(this.GetType()))
            myAbility = Abilities.Abilitieslist[this.GetType()];
        else
            myAbility = new Abilities.Ability(this.GetType(), currentLevel, false, ActionOnStart);
    }


    public virtual void Listener()
    {
        DetailsWindow.instance.ModifyTexts(information);

        DetailsWindow.instance.GenerateButtons(buttons);

        DetailsWindow.instance.SetLevelUpButton(Upgrade);
    }

    public abstract void ActionOnStart();
    
    public virtual void Upgrade()
    {
        if(currentLevel < buttons.Length-1)
        {
            NormalUpgrade();
        }
        else
        {
            NormalUpgrade();
            DetailsWindow.instance.DeactiveLevelButton();
            UnlockAbility();
        }
    }

    public void NormalUpgrade()
    {
        CheckPoints(allCosts[currentLevel]);
        currentLevel++;
        ChangeColor();

        myAbility.level = currentLevel;
    }


    public void CheckPoints(int cost)
    {
        var aux = CSVReader.LoadFromPictionary<int>("PlayerPoints");
        if (aux >= cost)
        {
            aux -= cost;
            CSVReader.SaveInPictionary<int>("PlayerPoints", aux);

            DetailsWindow.instance.RefreshPoints();
        }
        else
            print("No tienes puntos suficientes");
    }

    public virtual void UnlockAbility()
    {
        print("Desbloqueaste una habilidad");
        /*
         * Esta funcion se ejecuta cada que un boton llega a su nivel maximo y debe desbloquear otro boton
         * Los botones bloqueados deben tener el "level" en -1
         * Al activarlos su level cambia a 0, ya que deben ser comprados despues 
        */
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
public static class Abilities
{
    [System.Serializable]
    public class Ability
    {
        public int level;
        public bool active;

        public Ability(System.Type tipo, int level, bool active, System.Action onStart)
        {
            this.level = level;
            this.active = active;

            Abilitieslist.Add(tipo, this);
        }
    }

    public static Pictionarys<System.Type, Ability> Abilitieslist= new Pictionarys<System.Type, Ability>();
}

/* Se tiene que poder guardar los cambios en las habilidades
 * 
 * Deberia existir una funcion que sea virtual que defina el que pasara cuando un nivel empiece "ref: OnStart"
 * Esa funcion debe ser overrideada por los hijos, ya que cada uno debe modificar algo diferente (Armadura, vida, etc)
 * La clase debe ser guardada en un json. Esta clase = Abilities*/


