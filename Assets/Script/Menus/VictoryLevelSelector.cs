using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryLevelSelector : MonoBehaviour
{
    public MenuManager refMenuManager;
    public GameObject refSelectLevelMenu;
    public GameObject refMenuInGame;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        refMenuInGame.SetActive(false);
        refSelectLevelMenu.SetActive(true);

        refMenuManager.OpenCloseMenu();
    }

}
