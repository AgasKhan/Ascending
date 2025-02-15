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

    [System.Serializable]
    public struct SelectLevels 
    {
        public DoubleString texts;
        public Sprite previewImage;
        public int numberScene;
    }


    public SelectLevels[] preview;

    public GameObject[] menus;
    public GameObject[] subMenus;
    public Button[] levelButtons;
    public string firstLevel;
    public GameObject gamePausedMenu;
    public SceneChanger refSceneChanger;
    public AudioManager audioM;

    private int _currentMemuPrincipal = 0;
    private int _currentMemu = 0;
    //private int _currentPreview = 0;
    
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

        audioM = GetComponent<AudioManager>();
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
        

        if (!_inGame)
        {
            audioM.Play("MenuMusic");
        }

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
            if (Controllers.pause.down)
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
        DetailsWindow.ChangeAlpha(0, 0.1f);
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
      
        Time.timeScale = System.Convert.ToInt32(!gamePausedMenu.activeSelf);

        Controllers.MouseLock(!gamePausedMenu.activeSelf);

        GameManager.saveTime = !gamePausedMenu.activeSelf;

        Controllers.verticalMouse.enable = !gamePausedMenu.activeSelf;
        Controllers.horizontalMouse.enable = !gamePausedMenu.activeSelf;
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

        DetailsWindow.ChangeAlpha(1,0.1f);

        DetailsWindow.PreviewImage(true, preview[index].previewImage);

        DetailsWindow.SetMyButton(() => { SelectLevel(preview[index].numberScene.ToString()); }, true, "Play: " + preview[index].texts.superior);

        DetailsWindow.ActiveButtons(false);

        var aux = preview[index].texts;

        aux.inferior += "\n" + "Misiones incompletas:".RichText("color","red")+"\n";

        foreach (var item in Quests.SrchIncomplete(preview[index].numberScene))
        {
            aux.inferior += ("\n" + item.Description.superior.RichText("size", "16") + "\n"+ item.Description.inferior.RichText("size", "12") + "\n");
        }

        aux.inferior += "\n" + "Misiones completas:".RichText("color", "green")+"\n";

        foreach (var item in Quests.SrchComplete(preview[index].numberScene))
        {
            aux.inferior += ("\n" + item.Description.superior.RichText("size", "16") + "\n" + item.Description.inferior.RichText("size", "12") + "\n");
        }

        DetailsWindow.ModifyTexts(aux);       
    }

    public void BackToLobby()
    {
        foreach (var item in Quests.SrchIncomplete(BaseData.currentLevel))
        {
            item.active = false;
        }

        refSceneChanger.Load("Lobby");
    }


    public void ClickSound()
    {
        audioM.Play("Click");
    }

    public void ClickAccept()
    {
        audioM.Play("ClickAccept");
    }

    public void ClaimSound()
    {
        audioM.Play("Claim");
    }

    public void FireworksSound()
    {
        audioM.Play("Fireworks");
    }

}
