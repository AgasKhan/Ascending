using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.Audio;


public class MenuManager : MonoBehaviour
{
    static public MenuManager instance;

    public GameObject[] menus;
    public GameObject[] subMenus;
    public Button[] levelButtons;
    public GameObject[] previews;
    public Sprite[] previewImages;
    public Image imageToChange;
    public string firstLevel;
    public GameObject gamePausedMenu;
    public SceneChanger refSceneChanger;
    public AudioManager audioM;

    private int _currentMemuPrincipal = 0;
    private int _currentMemu = 0;
    private int _currentPreview = 0;
    
    //private string  _currentSubMemu= "GeneralOptionsButton";
    private bool _inGame = true;

    //para los eventos
    public Pictionarys<string, Action<GameObject>> eventListVoid = new Pictionarys<string, Action<GameObject>>();
    public Pictionarys<string, Action<GameObject, float>> eventListFloat = new Pictionarys<string, Action<GameObject, float>>();
    public Pictionarys<string, Action<GameObject, string>> eventListString = new Pictionarys<string, Action<GameObject, string>>();

    public Pictionarys<string, Action<Slider>> eventListSliderOn = new Pictionarys<string, Action<Slider>>();
    public Pictionarys<string, Action<Button>> eventListButtoOn = new Pictionarys<string, Action<Button>>();

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
                    aux.text = "Play Level  " + number.ToString();

                    levelButtons[i].onClick.RemoveAllListeners();
                    levelButtons[i].onClick.AddListener(() =>  //Funcion Lambda
                    {
                        SelectLevel(number.ToString());
                    });
                }
            }

        if (SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "Lobby")
        {
            _inGame = true;
        }

        else
        {
            _inGame = false;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Start()
    {
        /*
        foreach (var item in GetComponentsInChildren<Button>(true))
        {
            DebugPrint.Log("Nombre del boton: " + item.name.RichText("color", "green"));

            for (int i = 0; i < item.onClick.GetPersistentEventCount(); i++)
            {
                DebugPrint.Log("\tmetodo: " + item.onClick.GetPersistentMethodName(i).RichText("color", "yellow"));
            }

            if (eventListButtoOn.ContainsKey(item.name))
            {
                eventListButtoOn[item.name](item);
                eventListButtoOn.Remove(item.name);
                DebugPrint.Log("\t\tInicializado");
            }
            if (eventListVoid.ContainsKey(item.name))
            {
                item.Event();
            }
        }
        */
        audioM = GetComponent<AudioManager>();

        audioM.Play("MenuMusic");

        foreach (var item in GetComponentsInChildren<Slider>(true))
        {
            DebugPrint.Log("Nombre del Slider: " + item.name.RichText("color", "green"));

            for (int i = 0; i < item.onValueChanged.GetPersistentEventCount(); i++)
            {
                DebugPrint.Log("\tmetodo: " + item.onValueChanged.GetPersistentMethodName(i).RichText("color", "yellow"));
            }
            if (eventListSliderOn.ContainsKey(item.name))
            {
                eventListSliderOn[item.name](item);
                eventListSliderOn.Remove(item.name);
                DebugPrint.Log("\tInicializado");
            }
            if (eventListFloat.ContainsKey(item.name))
            {
                item.Event();
            }
        }
    }

    void Update()
    {
        if (_inGame)
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
        ClickSound();
        if (_currentMemu < subMenus.Length - 1)
        {
            subMenus[_currentMemu].SetActive(false);
            _currentMemu++;
            subMenus[_currentMemu].SetActive(true);
        }
    }

    public void GoToPreviousSubMenu()
    {
        ClickSound();
        if (_currentMemu > 0)
        {
            subMenus[_currentMemu].SetActive(false);
            _currentMemu--;
            subMenus[_currentMemu].SetActive(true);
        }
    }
    public void ChangeMenu(int index)
    {
        ClickSound();
        if (index != _currentMemuPrincipal)
        {
            menus[_currentMemuPrincipal].SetActive(false);
            menus[index].SetActive(true);
            _currentMemuPrincipal = index;
        }
    }


    public void ChangeSubMenu(int index)
    {
        ClickSound();
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
        ClickAccept();
        refSceneChanger.Load(firstLevel);
    }

    public void SelectLevel(string number)
    {
        /*var aux = this.GetComponentInChildren<TMP_Text>();
        string level = "";
        if (aux!=null)
            level = "Level_" + aux.text;*/
        ClickAccept();
        string level = "Level_" + number;

        // if(level != null)
        //Application.loadedLevelName

        if (_inGame)
            Controllers.eneable = false;


        refSceneChanger.Load(level);
    }

    public void CloseMainMenus()
    {
        ClickSound();
        menus[_currentMemuPrincipal].SetActive(false);
        menus[0].SetActive(true);
        _currentMemuPrincipal = 0;
    }


    public void ChangePreviews(int index)
    {
        ClickSound();
        if (index != _currentPreview)
        {
            previews[_currentPreview].SetActive(false);
            previews[index].SetActive(true);
            imageToChange.sprite = previewImages[index];
            _currentPreview = index;
        }
    }

    public void ClickSound()
    {
        audioM.Play("Click");
    }

    public void ClickAccept()
    {
        audioM.Play("ClickAccept");
    }
}
