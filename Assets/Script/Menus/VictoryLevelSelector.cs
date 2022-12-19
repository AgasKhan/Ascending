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

        foreach (var item in Quests.incomplete)
        {
            if (item.active && !item.update && item.chck())
                item.active = false;
        }

        if(LevelToUnlock > CSVReader.LoadFromPictionary<int>("LastUnlockedLevel"))
            CSVReader.SaveInPictionary<int>("LastUnlockedLevel", LevelToUnlock);

        _refMenuManager.refSceneChanger.Load("Lobby");
    }

}
