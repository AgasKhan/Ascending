using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryLevelSelector : MonoBehaviour
{
    MenuManager _refMenuManager;
    public int LevelToUnlock;

    private void Start()
    {
        _refMenuManager = MenuManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if(LevelToUnlock > CSVReader.LoadFromPictionary<int>("LastUnlockedLevel"))
            CSVReader.SaveInPictionary<int>("LastUnlockedLevel", LevelToUnlock);

        CSVReader.SaveInPictionary<int>("CurrentLevel", LevelToUnlock - 1);
        _refMenuManager.refSceneChanger.Load("Lobby");
    }

}
