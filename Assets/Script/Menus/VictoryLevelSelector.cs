using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryLevelSelector : MonoBehaviour
{
    MenuManager _refMenuManager;
    public GameObject refSelectLevelMenu;
    public GameObject refMenuInGame;

    private void Start()
    {
        _refMenuManager = MenuManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        refMenuInGame.SetActive(false);
        refSelectLevelMenu.SetActive(true);

        _refMenuManager.OpenCloseMenu();
    }

}
