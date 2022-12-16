using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillTreeManager : MonoBehaviour
{
    static public SkillTreeManager instance;
    
    //public GameObject[] currentAbilities;
    [SerializeField]
    AbilityButton[] allAbilities;
    [SerializeField]
    GameObject[] detailsWindow;
    [SerializeField]
    TextMeshProUGUI points;

    public Ability myAbility;
    
    private int currentDetailW = 0;

    public enum Ability
    {
        Charge,
        HitScan,

        TimeToArrive,

        InitialDaggers,

        HealthPoints,
        Armor,

        Speed,
        
        TimeToInteract,
        CallAllDaggers,

        InitialPower,
    }

    private void Awake()
    {
        instance = this;
    }

    public void OpenDetailWindow(string s)
    {
        for (int i = 0; i < detailsWindow.Length; i++)
        {
            if (detailsWindow[i].name == s)
            {
                detailsWindow[currentDetailW].SetActive(false);
                detailsWindow[i].SetActive(true);
            }
        }
    }
    public void OpenLockedDetails(string name, string message)
    {
        for (int i = 0; i < detailsWindow.Length; i++)
        {
            if (detailsWindow[i].name == name)
            {
                detailsWindow[currentDetailW].SetActive(false);
                detailsWindow[i].SetActive(true);
                var aux = detailsWindow[i].GetComponent<TextMeshProUGUI>();
                aux.text = message;
            }
        }
    }


    public AbilityButton FindAbilityButton(string s)
    {
        for (int i = 0; i < allAbilities.Length; i++)
        {
            if (allAbilities[i].myAbility.ToString() == s)
                return allAbilities[i];
        }
        return null;
    }

    public void UnlockAbility(string s)
    {
        var aux = FindAbilityButton(s);
        aux.myCanvasGroup.interactable = true;
        aux.myCanvasGroup.blocksRaycasts = true;
    }


    public void RefreshPoints()
    {
        if (points != null)
            points.text = CSVReader.LoadFromPictionary<int>("PlayerPoints").ToString();
    }

}
