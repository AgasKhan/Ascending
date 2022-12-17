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
        [TextArea(1, 6)]
        public string superior;

        [TextArea(3, 6)]
        public string inferior;
    }

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
        if (points >= cost)
        {
            points -= cost;
            CSVReader.SaveInPictionary<int>("PlayerPoints", points);

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

