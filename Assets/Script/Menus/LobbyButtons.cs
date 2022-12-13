using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyButtons : MonoBehaviour
{
    MenuManager myMenuManager;
    int lastLevelUnlocked;
    public GameObject[] skillButtons;

    void Awake()
    {
        myMenuManager = MenuManager.instance;

        myMenuManager.eventListVoid.AddRange(new Pictionarys<string, System.Action<GameObject>>()
        {
            {"rLevel", RestartLevel},
            {"nLevel", NextLevel},
            {"sLevel", SelectLevelLobby},
            {"bMenu", BackMenu}

        });

    }

    void BackMenu(GameObject g)
    {
        myMenuManager.refSceneChanger.Load("MainMenu");
    }
    void RestartLevel(GameObject g)
    {
        int aux= CSVReader.LoadFromPictionary<int>("CurrentLevel");
        myMenuManager.refSceneChanger.Load("Level_"+ aux.ToString());
    }
    void NextLevel(GameObject g)
    {
        int aux = CSVReader.LoadFromPictionary<int>("LastUnlockedLevel");
        myMenuManager.refSceneChanger.Load("Level_"+ aux.ToString());
    }
    void SelectLevelLobby(GameObject g)
    {
        myMenuManager.ChangeMenu(1);
    }


}
