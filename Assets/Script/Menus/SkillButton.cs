using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillButton : MonoBehaviour
{
    public enum Ability
    {
        TimeToAttract,
        Charge,
        HitScan,
        InitialDaggers,

        Speed,
        Armor,
        HealthPoints,
        Interact,
        AttractAll,

        InitialPower,
    }

    public Ability myAbility;
    public float quantity = 0;
    public bool activate = false;
    public int cost;
    public Button nextButton;
    public TextMeshProUGUI myCost;

    TextMeshProUGUI myImprovement;
    Button currentButton;
    public void Awake()
    {
        myImprovement = GetComponentInChildren<TextMeshProUGUI>();
        currentButton = GetComponent<Button>();

        myCost.text = cost.ToString() + " pts";

        if (quantity>0)
            myImprovement.text = "+ " + quantity.ToString();
        else if(quantity==0)
            myImprovement.text = "Unlock";
        else
            myImprovement.text = quantity.ToString();

        MenuManager.instance.RefreshPoints();
    }
    public void CheckPoints()
    {
        var aux = CSVReader.LoadFromPictionary<int>("PlayerPoints");
        if (aux >= cost)
        {
            aux -= cost;
            CSVReader.SaveInPictionary<int>("PlayerPoints", aux);

            if(quantity == 0)
                ImproveSkills(myAbility.ToString(), activate);
            else
                ImproveSkills(myAbility.ToString(), quantity);

            UnlockNextButton();
            MenuManager.instance.RefreshPoints();
        }
        else
            print("No tienes puntos suficientes");

    }

    void ImproveSkills(string ability, float points)
    {
        CSVReader.SaveInPictionary<float>(ability, points);
    }
    void ImproveSkills(string ability, bool b)
    {
        CSVReader.SaveInPictionary<bool>(ability, b);
    }

    public void UnlockNextButton()
    {
        if(currentButton!=null)
        {
            //currentButton.onClick.RemoveAllListeners();
            //var aux = currentButton.colors.disabledColor;
            //aux = Color.white;
            currentButton.interactable = false;
        }
            

        
        if(nextButton!=null)
            nextButton.interactable = true;
    }

}
