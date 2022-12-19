using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LevelUpButton : ButtonColorParent
{
    public TextMeshProUGUI costText;
    public TextMeshProUGUI improvementText;

    public string cost
    {
        set
        {
            costText.text = value;
        }
    }
    public string improvement
    {
        set
        {
            improvementText.text = value;
        }
    }

    protected override void Config()
    {
        base.Config();
        //MyAwakes += 
    }


    /*
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
            RefreshPoints();
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
    */

}
