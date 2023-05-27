using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    public static int playerPoints;

    public static LobbyManager instance;

    public TextMeshProUGUI pointsCounter;

    public RewardPopUp reward;

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
        instance = this;

        MenuManager.instance.eventListVoid.AddRange(new Pictionarys<string, System.Action<GameObject>>()
        {
            {"rLevel", RestartLevel},
            {"nLevel", NextLevel},
            {"sLevel", SelectLevelLobby},
            {"bMenu", BackMenu}

        });

        //playerPoints = CSVReader.LoadFromPictionary<int>("PlayerPoints", 21);

        List<DoubleString> message = new List<DoubleString>();

        //reclamo y lo guardo en mi lista
        for (int i = Quests.incomplete.Count - 1; i >= 0; i--)
        {
            message.Add(Quests.incomplete[i].Reward());
        }

        //limpio la lista de los vacios
        for (int i = message.Count-1; i >= 0; i--)
        {
            if (message[i].superior == "" || message[i].superior==null)
                message.RemoveAt(i);
        }

        //en caso de que tenga mensajes, llamo al pop up
        if(message.Count>0)
        {
            print("llamo al pop up");

            reward.SetReward(message);

            foreach (var item in message)
            {
                //muestro por consola las misiones completadas
                DebugPrint.Log(item.superior+"="+item.inferior);
            }
        }

    }

    private void Start()
    {
        DebugPrint.Log(Abilities.Abilitieslist.ToString());
        RefreshPoints();

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
        // aux += 1

        int aux = BaseData.lastLevelUnlocked;
        MenuManager.instance.refSceneChanger.Load("Level_" + aux.ToString());

        Quests.ChargeQuests(aux);
    }
    void SelectLevelLobby(GameObject g)
    {
        MenuManager.instance.ChangeMenu(1);
        //DetailsWindow.
    }

    public void RefreshPoints()
    {
        pointsCounter.text = playerPoints.ToString();
    }
    #endregion

    private void Update()
    {
        if (Controllers.jump.down)
        {
            AddPoints(10);
            RefreshPoints();
        }
    }

}
