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
        MyAwakes += MyAwake;
    }

    void MyAwake()
    {
        gameObject.SetActive(false);
    }
}
