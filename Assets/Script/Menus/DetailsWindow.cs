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

    Image[] levelsImage;

    [SerializeField]
    Button myLevelUpButton;

    int currentImLevel;


    private void Awake()
    {
        instance = this;
        RefreshPoints();
    }

    public void ModifyTexts(AbilitiesParent.DoubleString d)
    {
        myTitle.text = d.superior;
        myDescription.text = d.inferior;
    }

    public void GenerateButtons(AbilitiesParent.DoubleString[] d)
    {
        if (myUpgradesGrid.transform.childCount > 0)
            DeletePreviousButtons();

        transform.GetChild(3).gameObject.SetActive(true);

        for (int i = 0; i < d.Length; i++)
        {
            var aux = myUpgrade.GetComponent<LevelUpButton>();
            aux.cost = d[i].superior + " pts";
            aux.improvement = d[i].inferior;

            Instantiate(myUpgrade, myUpgradesGrid.transform);

        }
        /*
        for (int i = 0; i < myUpgradesGrid.transform.childCount; i++)
        {
            var aux = myUpgradesGrid.transform.GetChild(i).GetComponent<GameObject>();
            levelsImage[i] = aux.GetComponent<Image>();
        }*/

    }

    public void DeletePreviousButtons()
    {
        for (int i = 0; i < myUpgradesGrid.transform.childCount; i++)
        {
            Destroy(myUpgradesGrid.transform.GetChild(i).gameObject);
        }
    }

    public void SetLevelUpButton(System.Action myAction)
    {
        myLevelUpButton.interactable = true;
        myLevelUpButton.onClick.RemoveAllListeners();
        myLevelUpButton.onClick.AddListener(() =>
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

    public void ChangeLevelsColor()
    {
        levelsImage[currentImLevel].color = Color.white;
        currentImLevel++;
        levelsImage[currentImLevel].color = Color.magenta;
    }

    public void RefreshPoints()
    {
        pointsCounter.text = CSVReader.LoadFromPictionary<int>("PlayerPoints").ToString();
    }

}
