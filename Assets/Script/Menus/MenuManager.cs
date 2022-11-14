using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.Events;


public class MenuManager : MonoBehaviour
{
    static public MenuManager instance;

    public GameObject[] subMenus;
    public Button[] levelButtons;
    public string firstLevel;
    public GameObject gamePausedMenu;
    public SceneChanger refSceneChanger;


    private int _currentMemu = 0;
    //private string  _currentSubMemu= "GeneralOptionsButton";
    private bool _optionMenuActive = false;
    private bool _inGame = true;


    //para los eventos
    public Pictionarys<string, Action<GameObject>> eventListVoid = new Pictionarys<string, Action<GameObject>>();
    public Pictionarys<string, Action<GameObject, float>> eventListFloat = new Pictionarys<string, Action<GameObject, float>>();
    public Pictionarys<string, Action<GameObject, string>> eventListString = new Pictionarys<string, Action<GameObject, string>>();


    private void Awake()
    {
        instance = this;

        if (levelButtons != null)
            for (int i = 0; i < levelButtons.Length; i++)
            {
                int number = i + 1;
                TextMeshProUGUI aux = levelButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                
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

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            _inGame = true;
        }
            
        else
        {
            _inGame = false;
            Cursor.lockState = CursorLockMode.None;
        }
            


        foreach (var item in GetComponentsInChildren<Button>(true))
        {
            DebugPrint.Log("Nombre del boton: " + item.name.RichText("color", "green"));

            for (int i = 0; i < item.onClick.GetPersistentEventCount(); i++)
            {
                DebugPrint.Log("\tmetodo: " + item.onClick.GetPersistentMethodName(i).RichText("color", "yellow"));
            }
        }

        foreach (var item in GetComponentsInChildren<Slider>(true))
        {
            DebugPrint.Log("Nombre del Slider: " + item.name.RichText("color", "green"));

            for (int i = 0; i < item.onValueChanged.GetPersistentEventCount(); i++)
            {
                DebugPrint.Log("\tmetodo: " + item.onValueChanged.GetPersistentMethodName(i).RichText("color", "yellow"));
            }
        }
    }

    void Update()
    {
        if(_inGame)
        {
            if (Input.GetKeyDown("p") || Input.GetKeyDown(KeyCode.Escape))
            {
                OpenCloseMenu();
            }


            /*
            if (_optionMenuActive)
            {
                if (Input.GetKeyDown("h"))
                    GoToPreviousSubMenu();

                if (Input.GetKeyDown("j"))
                    GoToNextSubMenu();
            }
            */

            if (Controllers.locked.down)
                Controllers.MouseLock();
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

    public void ChangeSubMenu(int index)
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

        Cursor.lockState = (gamePausedMenu.activeSelf) ? CursorLockMode.None : CursorLockMode.Locked;
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

        if (_inGame)
            Controllers.eneable = false;


        refSceneChanger.Load(level);
    }

}
