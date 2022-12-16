using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DetailsWindow : MonoBehaviour
{
    public static DetailsWindow instance;

    [SerializeField]
    TextMeshProUGUI myTitle;
    [SerializeField]
    TextMeshProUGUI myDescription;

    [SerializeField]
    GameObject myUpgradesGrid;
    [SerializeField]
    GameObject myUpgrade;
    [SerializeField]
    Button myLevelUpButton;


    private void Awake()
    {
        instance = this;
    }

    public void ModifyTexts(AbilitiesParent.DoubleString d)
    {
        myTitle.text = d.superior;
        myDescription.text = d.inferior;
    }

    public void GenerateButtons(AbilitiesParent.DoubleString[] d)
    {
        for (int i = 0; i < d.Length; i++)
        {
            var aux = myUpgrade.GetComponent<LevelUpButton>();
            aux.cost = d[i].superior;
            aux.improvement = d[i].inferior;

            Instantiate(myUpgrade, myUpgradesGrid.transform);
        }
    }

    public void SetLevelUpButton()
    {
        myLevelUpButton.onClick.RemoveAllListeners();
        myLevelUpButton.onClick.AddListener(() =>
        {

        }

        );
    }

}
