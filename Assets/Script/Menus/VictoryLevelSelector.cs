using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryLevelSelector : MonoBehaviour
{
    MenuManager _refMenuManager;
    public GameObject refSelectLevelMenu;

    private void Start()
    {
        _refMenuManager = MenuManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        _refMenuManager.gameObject.SetActive(true);

        refSelectLevelMenu.SetActive(true);

        _refMenuManager.OpenCloseMenu();
    }

}
