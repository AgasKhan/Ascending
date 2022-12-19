using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static int playerPoints;

    // Duda: Crear todas las misiones a la vez? (Start o Awake del main menu)
   
    public static void AddPoints(int p)
    {
        playerPoints += p;
        CSVReader.SaveInPictionary<int>("PlayerPoints", playerPoints);
    }

    #region lobby buttons

    //public GameObject[] skillButtons;

    void Awake()
    {
        MenuManager.instance.eventListVoid.AddRange(new Pictionarys<string, System.Action<GameObject>>()
        {
            {"rLevel", RestartLevel},
            {"nLevel", NextLevel},
            {"sLevel", SelectLevelLobby},
            {"bMenu", BackMenu}

        });

        //playerPoints = CSVReader.LoadFromPictionary<int>("PlayerPoints", 21);


        foreach (var item in Quests.incomplete)
        {
            item.Reward();
        }

    }

    private void Start()
    {
        DebugPrint.Log(Abilities.Abilitieslist.ToString());
    }

    void BackMenu(GameObject g)
    {
        MenuManager.instance.refSceneChanger.Load("MainMenu");
    }
    void RestartLevel(GameObject g)
    {
        int aux = BaseData.currentLevel;
        MenuManager.instance.refSceneChanger.Load("Level_" + aux.ToString());

        Quests.ChargeQuests(aux);
    }
    void NextLevel(GameObject g)
    {
        // int aux = CSVReader.LoadFromPictionary<int>("CurrentLevel");
        // aux +1

        int aux = CSVReader.LoadFromPictionary<int>("LastUnlockedLevel");
        MenuManager.instance.refSceneChanger.Load("Level_" + aux.ToString());

        Quests.ChargeQuests(aux);
    }
    void SelectLevelLobby(GameObject g)
    {
        MenuManager.instance.ChangeMenu(1);
    }
    #endregion

    private void Update()
    {
        if (Controllers.jump.down)
            AddPoints(10);
    }

}
