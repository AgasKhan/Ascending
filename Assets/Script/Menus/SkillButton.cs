using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillButton : MonoBehaviour
{
    public int abilityNumber;
    public float quantity;
    public bool boolIsNeeded = false;
    public int cost;
    public Button nextButton;

    TextMeshProUGUI myText;
    Button currentButton;
    public void Awake()
    {
        myText = GetComponentInChildren<TextMeshProUGUI>();
        currentButton = GetComponent<Button>();

        if(quantity>0)
            myText.text = "+ " + quantity.ToString();
        else
            myText.text = quantity.ToString();

        MenuManager.instance.RefreshPoints();
    }
    public void CheckPoints()
    {
        var aux = CSVReader.LoadFromPictionary<int>("PlayerPoints");
        if (aux >= cost)
        {
            aux -= cost;
            CSVReader.SaveInPictionary<int>("PlayerPoints", aux);

            if(boolIsNeeded)
                ImproveSkills(abilityNumber,boolIsNeeded);
            else
                ImproveSkills(new Vector2(abilityNumber, quantity));

            UnlockNextButton();
            MenuManager.instance.RefreshPoints();
        }
        else
            print("No tienes puntos suficientes");

    }

    void ImproveSkills(Vector2 v)
    {
        CSVReader.SaveInPictionary<float>("Ability_" + (v.x.ToString()), v.y);
    }
    void ImproveSkills(int ability, bool b)
    {
        CSVReader.SaveInPictionary<bool>("Ability_" + (ability.ToString()), b);
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
