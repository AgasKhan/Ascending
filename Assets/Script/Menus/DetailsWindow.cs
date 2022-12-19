using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DetailsWindow : MonoBehaviour
{
    
    public static DetailsWindow instance;
    public TextMeshProUGUI pointsCounter;

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

    [SerializeField]
    LevelUpButton[] buttoncitos;

    private void Awake()
    {
        instance = this;
        RefreshPoints();
    }

    public static void ModifyTexts(DoubleString d)
    {
        instance.myTitle.text = d.superior;
        instance.myDescription.text = d.inferior;
    }

    public static void GenerateButtons(DoubleString[] d)
    {        
        for (int i = 0; i < instance.myUpgradesGrid.transform.childCount; i++)
        {
            instance.myUpgradesGrid.transform.GetChild(i).gameObject.SetActive(false);
        }

        instance.transform.GetChild(3).gameObject.SetActive(true);

        for (int i = 0; i < d.Length; i++)
        {
            instance.buttoncitos[i].gameObject.SetActive(true);

            var aux = instance.buttoncitos[i];
            aux.cost = d[i].superior + " pts";
            aux.improvement = d[i].inferior;

            aux.ChangeColor(i+1, d.Length);
        }

        /*
        for (int i = 0; i < myUpgradesGrid.transform.childCount; i++)
        {
            var aux = myUpgradesGrid.transform.GetChild(i).GetComponent<GameObject>();
            levelsImage[i] = aux.GetComponent<Image>();
        }*/

    }

    public static void SetLevelUpButton(System.Action myAction, bool interact)
    {
        instance.myLevelUpButton.interactable = interact;
        instance.myLevelUpButton.onClick.RemoveAllListeners();
        instance.myLevelUpButton.onClick.AddListener(() =>
        {
            myAction();
            //ChangeLevelsColor();
        }
        );
    }
    public void DeactiveLevelButton()
    {
        myLevelUpButton.interactable = false;
    }

    public void RefreshPoints()
    {
        pointsCounter.text = CSVReader.LoadFromPictionary<int>("PlayerPoints").ToString();
    }

}
