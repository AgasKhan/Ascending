using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public GameObject[] subMenus;
    public Button[] levelButtons;
    public string firstLevel;
    public GameObject gamePausedMenu;
    public SceneChanger refSceneChanger;

    private int _currentMemu = 0;
    //private string  _currentSubMemu= "GeneralOptionsButton";
    private bool _optionMenuActive = false;
    private bool _inGame = true;
    

    private void Start()
    {
        if (levelButtons != null)
            for (int i = 0; i < levelButtons.Length; i++)
            {
                int number = i + 1;
                TMP_Text aux = levelButtons[i].GetComponentInChildren<TMP_Text>();
                
                if (aux != null)
                {
                    aux.text = number.ToString();

                    levelButtons[i].onClick.RemoveAllListeners();
                    levelButtons[i].onClick.AddListener(() =>  //Funcion Lambda
                    {
                        SelectLevel(number.ToString());
                    });
                }
            }
    }

    void Update()
    {
        if(_inGame)
        {
            if (Input.GetKeyDown("p"))
            {
                OpenCloseMenu();
            }

            if (_optionMenuActive)
            {
                if (Input.GetKeyDown("h"))
                    GoToPreviousSubMenu();

                if (Input.GetKeyDown("j"))
                    GoToNextSubMenu();
            }

            if (Controllers.locked.down)
                Cursor.lockState = (Cursor.lockState == CursorLockMode.None) ? CursorLockMode.Locked : CursorLockMode.None;
        }
        
    }

    public void GoToNextSubMenu()
    {
        if (_currentMemu < subMenus.Length - 1)
        {
            subMenus[_currentMemu].SetActive(false);
            _currentMemu++;
            subMenus[_currentMemu].SetActive(true);
        }
    }

    public void GoToPreviousSubMenu()
    {

        if (_currentMemu > 0)
        {
            subMenus[_currentMemu].SetActive(false);
            _currentMemu--;
            subMenus[_currentMemu].SetActive(true);
        }

    }

    public void ChangeOptions(int index)
    {
        if (index != _currentMemu)
        {
            subMenus[_currentMemu].SetActive(false);
            subMenus[index].SetActive(true);
            _currentMemu = index;
        }
    }

    public void OpenCloseMenu()
    {
        gamePausedMenu.SetActive(!gamePausedMenu.activeSelf);
        Debug.Log(gamePausedMenu.activeSelf);
        Time.timeScale = System.Convert.ToInt32(!gamePausedMenu.activeSelf);
    }

    public void StartGame()
    {
        refSceneChanger.Load(firstLevel);
    }

    public void SelectLevel(string number)
    {
        /*var aux = this.GetComponentInChildren<TMP_Text>();
        string level = "";
        if (aux!=null)
            level = "Level_" + aux.text;*/

        string level = "Level_" + number;

        // if(level != null)
        //Application.loadedLevelName

        Controllers.eneable = false;
        refSceneChanger.Load(level);
    }

}
